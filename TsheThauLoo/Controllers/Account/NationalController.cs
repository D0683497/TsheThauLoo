using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account.National;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.National;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/account/national")]
    public class NationalController : ControllerBase
    {
        private readonly ILogger<NationalController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public NationalController(
            ILogger<NationalController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AuthAuthorize]
        [HttpGet(Name = nameof(National))]
        public async Task<ActionResult<NationalDto>> National()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<NationalDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpPost(Name = nameof(EditNational))]
        public async Task<ActionResult<NationalDto>> EditNational([FromBody] NationalEditDto dto)
        {
            NationalEditDtoValidator validator = new NationalEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Users
                    .SingleOrDefaultAsync(x => x.Id == userId);

                #region 驗證

                switch (entity.IdentityConfirmed)
                {
                    case true:
                    {
                        if (entity.NationalId != dto.NationalId)
                        {
                            result.Errors.Add(new ValidationFailure("nationalId", "身份證字號禁止修改"));
                        }
                        if (entity.Name != dto.Name)
                        {
                            result.Errors.Add(new ValidationFailure("name", "姓名禁止修改"));
                        }
                        if (entity.Gender != dto.Gender)
                        {
                            result.Errors.Add(new ValidationFailure("gender", "性別禁止修改"));
                        }
                        if (entity.DateOfBirth != dto.DateOfBirth)
                        {
                            result.Errors.Add(new ValidationFailure("dateOfBirth", "生日禁止修改"));
                        }
                        break;
                    }
                    case false:
                    {
                        if (entity.NationalId != dto.NationalId && !string.IsNullOrEmpty(dto.NationalId))
                        {
                            if (await _dbContext.Users.AnyAsync(x => x.NationalId == dto.NationalId.ToUpper()))
                            {
                                result.Errors.Add(new ValidationFailure("nationalId", "身份證字號已經被使用"));
                            }
                        }
                        break;
                    }
                }
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                #endregion

                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Users.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<NationalDto>(updateEntity);
                return CreatedAtAction(nameof(National), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpGet("verify", Name = nameof(NationalVerify))]
        public async Task<ActionResult<NationalVerifyDto>> NationalVerify()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.NationalVerify)
                .Include(x => x.NationalVerify.NationalVerifyFiles)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<NationalVerifyDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpPost("verify", Name = nameof(EditNationalVerify))]
        public async Task<ActionResult<NationalVerifyDto>> EditNationalVerify([FromBody] NationalEditVerifyDto dto)
        {
            NationalEditVerifyDtoValidator validator = new NationalEditVerifyDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _dbContext.Users
                    .Include(x => x.NationalVerify)
                    .Include(x => x.NationalVerify.NationalVerifyFiles)
                    .SingleOrDefaultAsync(x => x.Id == userId);
                if (user.IdentityConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "已通過實名驗證", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, user.NationalVerify);
                _dbContext.NationalVerifies.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<NationalVerifyDto>(user);
                return CreatedAtAction(nameof(NationalVerify), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost("verify/files", Name = nameof(CreateNationalVerifyFile))]
        public async Task<ActionResult<FileDto>> CreateNationalVerifyFile([FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _dbContext.Users
                    .Include(x => x.NationalVerify)
                    .Include(x => x.NationalVerify.NationalVerifyFiles)
                    .SingleOrDefaultAsync(x => x.Id == userId);
                if (user.IdentityConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "已通過實名驗證", statusCode: 403);
                }
                var entity = _mapper.Map<NationalVerifyFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        user.NationalVerify.NationalVerifyFiles.Add(entity);
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

                var returnDto = _mapper.Map<FileDto>(entity);
                var routeValues = new {fileId = returnDto.Id};
                return CreatedAtAction(nameof(NationalVerifyFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpPost("verify/files/{fileId}", Name = nameof(EditNationalVerifyFile))]
        public async Task<ActionResult<FileDto>> EditNationalVerifyFile([FromRoute] string fileId, [FromBody] FileEditDto dto)
        {
            FileEditDtoValidator validator = new FileEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _dbContext.Users
                    .Include(x => x.NationalVerify)
                    .Include(x => x.NationalVerify.NationalVerifyFiles)
                    .SingleOrDefaultAsync(x => x.Id == userId);
                if (user.IdentityConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "已通過實名驗證", statusCode: 403);
                }
                var entity = user.NationalVerify.NationalVerifyFiles
                    .SingleOrDefault(x => x.NationalVerifyFileId == fileId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.NationalVerifyFiles.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<FileDto>(updateEntity);
                var routeValues = new {fileId = returnDto.Id};
                return CreatedAtAction(nameof(NationalVerifyFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpGet("verify/files/{fileId}", Name = nameof(NationalVerifyFile))]
        public async Task<ActionResult<FileDto>> NationalVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.NationalVerify)
                .Include(x => x.NationalVerify.NationalVerifyFiles)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var entity = user.NationalVerify.NationalVerifyFiles
                .SingleOrDefault(x => x.NationalVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<FileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpGet("verify/files/{fileId}/download", Name = nameof(DownloadNationalVerifyFile))]
        public async Task<IActionResult> DownloadNationalVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.NationalVerify)
                .Include(x => x.NationalVerify.NationalVerifyFiles)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var entity = user.NationalVerify.NationalVerifyFiles
                .SingleOrDefault(x => x.NationalVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize]
        [HttpDelete("verify/files/{fileId}", Name = nameof(DeleteNationalVerifyFile))]
        public async Task<IActionResult> DeleteNationalVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .Include(x => x.NationalVerify)
                .Include(x => x.NationalVerify.NationalVerifyFiles)
                .SingleOrDefaultAsync(x => x.Id == userId);
            if (user.IdentityConfirmed)
            {
                return Problem(title: "禁止修改", detail: "已通過實名驗證", statusCode: 403);
            }
            var entity = user.NationalVerify.NationalVerifyFiles
                .SingleOrDefault(x => x.NationalVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.NationalVerifyFiles.Remove(entity);
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