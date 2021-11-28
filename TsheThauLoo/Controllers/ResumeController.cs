using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/resumes")]
    public class ResumeController: ControllerBase
    {
        private readonly ILogger<ResumeController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public ResumeController(
            ILogger<ResumeController> logger, 
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AuthAuthorize]
        [HttpGet(Name = nameof(GetResumes))]
        public async Task<ActionResult<IEnumerable<FileDto>>> GetResumes([FromQuery] PaginationResourceParameters parameters, [FromQuery] bool archive)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entities = await _dbContext.FileResumes
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .Where(x => x.IsArchive == archive)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<FileDto>>(entities);
            
            #region 分頁資訊

            var length = await _dbContext.FileResumes
                .Where(x => x.IsArchive == archive)
                .CountAsync();
            var paginationMetadata = new
            {
                pageLength = length, // 總資料數
                pageSize = parameters.PageSize, // 一頁的項目數
                pageIndex = parameters.PageIndex, // 目前頁碼
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            #endregion
            
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("{resumeId}", Name = nameof(GetResume))]
        public async Task<IActionResult> GetResume([FromRoute] string resumeId, [FromQuery] string userId)
        {
            userId ??= User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.FileResumes
                .AsNoTracking()
                .Where(x => x.ApplicationUserId == userId)
                .SingleOrDefaultAsync(x => x.FileResumeId == resumeId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost(Name = nameof(CreateResume))]
        public async Task<IActionResult> CreateResume([FromForm] FileCreateDto dto)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .Include(x => x.FileResumes)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var entity = _mapper.Map<FileResume>(dto);
            
            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    using (var stream = new FileStream(entity.Path, FileMode.Create))
                    {
                        await dto.FileData.CopyToAsync(stream);
                    }
                    user.FileResumes.Add(entity);
                    _dbContext.Users.Update(user);
                    await _dbContext.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (IOException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (DbUpdateException)
                {
                    System.IO.File.Delete(entity.Path);
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            #endregion
            
            var routeValues = new {resumeId = entity.FileResumeId};
            var returnDto = _mapper.Map<FileDto>(entity);
            return CreatedAtAction(nameof(GetResume), routeValues, returnDto);
        }
        
        [AuthAuthorize]
        [HttpPost("{resumeId}", Name = nameof(EditResume))]
        public async Task<IActionResult> EditResume([FromRoute] string resumeId, [FromBody] FileEditDto dto)
        {
            FileEditDtoValidator validator = new FileEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.FileResumes
                    .Where(x => x.ApplicationUserId == userId)
                    .SingleOrDefaultAsync(x => x.FileResumeId == resumeId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.FileResumes.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<FileDto>(updateEntity);
                return CreatedAtRoute(nameof(GetResume), new {resumeId = returnDto.Id}, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpDelete("{resumeId}", Name = nameof(DeleteResume))]
        public async Task<IActionResult> DeleteResume([FromRoute] string resumeId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.FileResumes
                .Where(x => x.ApplicationUserId == userId)
                .SingleOrDefaultAsync(x => x.FileResumeId == resumeId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.FileResumes.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    System.IO.File.Delete(entity.Path);
                    await transaction.CommitAsync();
                }
                catch (IOException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (DbUpdateException)
                {
                    System.IO.File.Delete(entity.Path);
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            #endregion
            
            return NoContent();
        }
    }
}
