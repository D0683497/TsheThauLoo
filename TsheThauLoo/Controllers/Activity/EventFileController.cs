using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize(Roles = "Administrator")]
    [Route("api/events/{eventId}/files")]
    public class EventFileController : ControllerBase
    {
        private readonly ILogger<EventFileController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public EventFileController(
            ILogger<EventFileController> logger,
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost(Name = nameof(CreateEventFile))]
        public async Task<IActionResult> CreateEventFile([FromRoute] string eventId, [FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                #region 驗證
                
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }

                #endregion
               
                var act = await _dbContext.Events
                    .Include(x => x.EventFiles)
                    .SingleOrDefaultAsync(x => x.EventId == eventId);
                if (act == null)
                {
                    return NotFound();
                }

                var entity = _mapper.Map<EventFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        act.EventFiles.Add(entity);
                        _dbContext.Events.Update(act);
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
                
                var routeValues = new {eventId = act.EventId, fileId = entity.EventFileId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(EventFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{fileId}", Name = nameof(EventFile))]
        public async Task<IActionResult> EventFile([FromRoute] string eventId, [FromRoute] string fileId)
        {
            var entity = await _dbContext.EventFiles
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.EventId == eventId && x.EventFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{fileId}", Name = nameof(EditEventFile))]
        public async Task<IActionResult> EditEventFile([FromRoute] string eventId, [FromRoute] string fileId, [FromBody] FileEditDto dto)
        {
            FileEditDtoValidator validator = new FileEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                #region 驗證
                
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }

                #endregion
                
                var entity = await _dbContext.EventFiles
                    .SingleOrDefaultAsync(x => x.EventId == eventId && x.EventFileId == fileId);
                if (entity == null)
                {
                    return NotFound();
                }
                
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.EventFiles.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {eventId = entity.EventId, fileId = entity.EventFileId};
                var returnDto = _mapper.Map<FileDto>(updateEntity);
                return CreatedAtAction(nameof(EventFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{fileId}", Name = nameof(DeleteEventFile))]
        public async Task<IActionResult> DeleteEventFile([FromRoute] string eventId, [FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var administrator = await _dbContext.Administrators
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
            #region 驗證
                
            if (!administrator.AdministratorConfirmed)
            {
                return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
            }

            #endregion
            
            var entity = await _dbContext.EventFiles
                .SingleOrDefaultAsync(x => x.EventId == eventId && x.EventFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            
            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.EventFiles.Remove(entity);
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