using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account.Profile.Administrator;
using TsheThauLoo.Dtos.Account.Profile.Alumnus;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Profile;
using TsheThauLoo.Validator.Account.Register;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize(Roles = "Alumnus")]
    [Route("api/account/alumnus")]
    public class AlumnusController : ControllerBase
    {
        private readonly ILogger<AlumnusController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AlumnusController(
            ILogger<AlumnusController> logger, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager, 
            TsheThauLooDbContext dbContext, 
            IConfiguration configuration, 
            IMailService mailService)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _mailService = mailService;
        }
        
        [AllowAnonymous]
        [HttpPost("register", Name = nameof(AlumnusRegister))]
        public async Task<IActionResult> AlumnusRegister([FromBody] AlumnusRegisterDto dto)
        {
            AlumnusRegisterDtoValidator validator = new AlumnusRegisterDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region 驗證重複

                if (await _userManager.Users.AnyAsync(x => x.UserName == dto.UserName))
                {
                    result.Errors.Add(new ValidationFailure("userName", "使用者名稱已經被使用"));
                }
                if (await _userManager.Users.AnyAsync(x => x.Email == dto.Email))
                {
                    result.Errors.Add(new ValidationFailure("email", "電子郵件已經被使用"));
                }
                if (!string.IsNullOrEmpty(dto.PhoneNumber))
                {
                    if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber))
                    {
                        result.Errors.Add(new ValidationFailure("phoneNumber", "手機號碼已經被使用"));
                    }
                }
                if (!string.IsNullOrEmpty(dto.NationalId))
                {
                    if (await _userManager.Users.AnyAsync(x => x.NationalId == dto.NationalId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("nationalId", "身份證字號已經被使用"));
                    }
                }

                #endregion
                
                if (result.IsValid)
                {
                    var entity = _mapper.Map<ApplicationUser>(dto);
                    
                    await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            #region 建立使用者

                            if (await _userManager.CreateAsync(entity, dto.Password) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加 Claim

                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, entity.Id),
                                new Claim(ClaimTypes.Name, entity.UserName),
                                new Claim(ClaimTypes.Email, entity.Email),
                                new Claim(ClaimTypes.Sid, entity.SecurityStamp)
                            };

                            if (await _userManager.AddClaimsAsync(entity, claims) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region 添加角色

                            if (await _userManager.AddToRoleAsync(entity, "Alumnus") != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            await transaction.CommitAsync();
                        }
                        catch (DbUpdateException)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                    
                    #region 寄信

                    var link = $"{_configuration["FrontendUrl"]}/account/email/confirm" + 
                               $"?userId={Uri.EscapeDataString(entity.Id)}" + 
                               $"&token={Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(entity))}";

                    await _mailService.SendEmailConfirmAsync(entity.Email, entity.Email, link, true);

                    #endregion
                    
                    var returnDto = _mapper.Map<AlumnusProfileDto>(entity);
                    return CreatedAtAction(nameof(AlumnusProfile), null, returnDto);
                }
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpGet("profile", Name = nameof(AlumnusProfile))]
        public async Task<ActionResult<AlumnusProfileDto>> AlumnusProfile()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.UserPhoto)
                .Include(x => x.Alumnus)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<AlumnusProfileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpGet("profile/info", Name = nameof(AlumnusInfo))]
        public async Task<ActionResult<AlumnusInfoDto>> AlumnusInfo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Alumni
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<AlumnusInfoDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpPost("profile/info", Name = nameof(AlumnusEditInfo))]
        public async Task<ActionResult<AlumnusInfoDto>> AlumnusEditInfo([FromBody] AlumnusEditInfoDto dto)
        {
            AlumnusEditInfoDtoValidator validator = new AlumnusEditInfoDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Alumni
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (entity.AlumnusConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "校友已驗證", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Alumni.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<AlumnusInfoDto>(updateEntity);
                return CreatedAtAction(nameof(AlumnusInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpGet("profile/verify", Name = nameof(AlumnusVerify))]
        public async Task<ActionResult<AlumnusVerifyDto>> AlumnusVerify()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Alumni
                .AsNoTracking()
                .Include(x => x.AlumnusVerify)
                .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<AlumnusVerifyDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpPost("profile/verify", Name = nameof(AlumnusEditVerify))]
        public async Task<ActionResult<AlumnusVerifyDto>> AlumnusEditVerify([FromBody] AlumnusEditVerifyDto dto)
        {
            AlumnusEditVerifyDtoValidator validator = new AlumnusEditVerifyDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var alumnus = await _dbContext.Alumni
                    .Include(x => x.AlumnusVerify)
                    .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (alumnus.AlumnusConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "校友已驗證", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, alumnus.AlumnusVerify);
                _dbContext.AlumnusVerifies.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<AlumnusVerifyDto>(alumnus);
                return CreatedAtAction(nameof(AlumnusVerify), null, returnDto);
                
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost("profile/verify/files", Name = nameof(AlumnusCreateVerifyFile))]
        public async Task<ActionResult<FileDto>> AlumnusCreateVerifyFile([FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var alumnus = await _dbContext.Alumni
                    .Include(x => x.AlumnusVerify)
                    .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (alumnus.AlumnusConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "校友已驗證", statusCode: 403);
                }
                var entity = _mapper.Map<AlumnusVerifyFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        alumnus.AlumnusVerify.AlumnusVerifyFiles.Add(entity);
                        _dbContext.Alumni.Update(alumnus);
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
                return CreatedAtAction(nameof(AlumnusVerifyFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpPost("profile/verify/files/{fileId}", Name = nameof(AlumnusEditVerifyFile))]
        public async Task<ActionResult<FileDto>> AlumnusEditVerifyFile([FromRoute] string fileId, [FromBody] FileEditDto dto)
        {
            FileEditDtoValidator validator = new FileEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var alumnus = await _dbContext.Alumni
                    .Include(x => x.AlumnusVerify)
                    .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (alumnus.AlumnusConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "校友已驗證", statusCode: 403);
                }
                var entity = alumnus.AlumnusVerify.AlumnusVerifyFiles
                    .SingleOrDefault(x => x.AlumnusVerifyFileId == fileId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.AlumnusVerifyFiles.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<FileDto>(updateEntity);
                var routeValues = new {fileId = returnDto.Id};
                return CreatedAtAction(nameof(AlumnusVerifyFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpGet("profile/verify/files/{fileId}", Name = nameof(AlumnusVerifyFile))]
        public async Task<ActionResult<FileDto>> AlumnusVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var alumnus = await _dbContext.Alumni
                .AsNoTracking()
                .Include(x => x.AlumnusVerify)
                .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var entity = alumnus.AlumnusVerify.AlumnusVerifyFiles
                .SingleOrDefault(x => x.AlumnusVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<FileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpGet("profile/verify/files/{fileId}/download", Name = nameof(AlumnusDownloadVerifyFile))]
        public async Task<IActionResult> AlumnusDownloadVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var alumnus = await _dbContext.Alumni
                .AsNoTracking()
                .Include(x => x.AlumnusVerify)
                .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var entity = alumnus.AlumnusVerify.AlumnusVerifyFiles
                .SingleOrDefault(x => x.AlumnusVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize(Roles = "Alumnus")]
        [HttpDelete("profile/verify/files/{fileId}", Name = nameof(AlumnusDeleteVerifyFile))]
        public async Task<IActionResult> AlumnusDeleteVerifyFile([FromRoute] string fileId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var alumnus = await _dbContext.Alumni
                .Include(x => x.AlumnusVerify)
                .Include(x => x.AlumnusVerify.AlumnusVerifyFiles)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (alumnus.AlumnusConfirmed)
            {
                return Problem(title: "禁止修改", detail: "校友已驗證", statusCode: 403);
            }
            var entity = alumnus.AlumnusVerify.AlumnusVerifyFiles
                .SingleOrDefault(x => x.AlumnusVerifyFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.AlumnusVerifyFiles.Remove(entity);
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