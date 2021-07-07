using System;
using System.Collections.Generic;
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
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Profile;
using TsheThauLoo.Validator.Account.Register;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize(Roles = "Administrator")]
    [Route("api/account/administrator")]
    public class AdministratorController : ControllerBase
    {
        private readonly ILogger<AdministratorController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public AdministratorController(
            ILogger<AdministratorController> logger, 
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
        [HttpPost("register", Name = nameof(AdministratorRegister))]
        public async Task<IActionResult> AdministratorRegister([FromBody] AdministratorRegisterDto dto)
        {
            AdministratorRegisterDtoValidator validator = new AdministratorRegisterDtoValidator();
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
                if (!string.IsNullOrEmpty(dto.NetworkId))
                {
                    if (await _userManager.Users.AnyAsync(x => 
                        x.Administrator.NetworkId == dto.NetworkId.ToUpper() || 
                        x.Employee.NetworkId == dto.NetworkId.ToUpper() || 
                        x.Student.NetworkId == dto.NetworkId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("networkId", "證號已經被使用"));
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

                            if (await _userManager.AddToRoleAsync(entity, "Administrator") != IdentityResult.Success)
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

                    var returnDto = _mapper.Map<AdministratorProfileDto>(entity);
                    return CreatedAtAction(nameof(AdministratorProfile), null, returnDto);
                }
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpGet("profile", Name = nameof(AdministratorProfile))]
        public async Task<ActionResult<AdministratorProfileDto>> AdministratorProfile()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.UserPhoto)
                .Include(x => x.Administrator)
                .Include(x => x.Administrator.Responsibilities)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<AdministratorProfileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpGet("profile/info", Name = nameof(AdministratorInfo))]
        public async Task<ActionResult<AdministratorInfoDto>> AdministratorInfo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Administrators
                .AsNoTracking()
                .Include(x => x.Responsibilities)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<AdministratorInfoDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("profile/info", Name = nameof(AdministratorEditInfo))]
        public async Task<ActionResult<AdministratorInfoDto>> AdministratorEditInfo([FromBody] AdministratorEditInfoDto dto)
        {
            AdministratorEditInfoDtoValidator validator = new AdministratorEditInfoDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Administrators
                    .Include(x => x.Responsibilities)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (!entity.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }

                #region 驗證重複

                if (entity.NetworkId != dto.NetworkId && !string.IsNullOrEmpty(dto.NetworkId))
                {
                    if (await _userManager.Users.AnyAsync(x => x.Administrator.NetworkId == dto.NetworkId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("networkId", "證號已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }

                #endregion

                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Administrators.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<AdministratorInfoDto>(updateEntity);
                return CreatedAtAction(nameof(AdministratorInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("profile/info/responsibilities", Name = nameof(AdministratorCreateResponsibility))]
        public async Task<ActionResult<AdministratorInfoDto>> AdministratorCreateResponsibility([FromBody] ResponsibilityEditDto dto)
        {
            ResponsibilityEditDtoValidator validator = new ResponsibilityEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .Include(x => x.Responsibilities)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止建立", detail: "管理員尚未驗證", statusCode: 403);
                }
                var entity = _mapper.Map<Responsibility>(dto);
                administrator.Responsibilities.Add(entity);
                _dbContext.Administrators.Update(administrator);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<AdministratorInfoDto>(administrator);
                return CreatedAtAction(nameof(AdministratorInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }

        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("profile/info/responsibilities/{responsibilityId}", Name = nameof(AdministratorEditResponsibility))]
        public async Task<ActionResult<AdministratorInfoDto>> AdministratorEditResponsibility([FromRoute] string responsibilityId, [FromBody] ResponsibilityEditDto dto)
        {
            ResponsibilityEditDtoValidator validator = new ResponsibilityEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .Include(x => x.Responsibilities)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                var entity = administrator.Responsibilities
                    .SingleOrDefault(x => x.ResponsibilityId == responsibilityId);
                if (entity == null)
                {
                    return NotFound();
                }
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Responsibilities.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<AdministratorInfoDto>(administrator);
                return CreatedAtAction(nameof(AdministratorInfo), null, returnDto);
                
            }
            return BadRequest(result.Errors);
        }

        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("profile/info/responsibilities/{responsibilityId}", Name = nameof(AdministratorDeleteResponsibility))]
        public async Task<ActionResult<AdministratorInfoDto>> AdministratorDeleteResponsibility([FromRoute] string responsibilityId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var administrator = await _dbContext.Administrators
                .Include(x => x.Responsibilities)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var entity = administrator.Responsibilities
                .SingleOrDefault(x => x.ResponsibilityId == responsibilityId);
            if (entity == null)
            {
                return NotFound();
            }
            if (!administrator.AdministratorConfirmed)
            {
                return Problem(title: "禁止刪除", detail: "管理員尚未驗證", statusCode: 403);
            }
            _dbContext.Responsibilities.Remove(entity);
            await _dbContext.SaveChangesAsync();
            var returnDto = _mapper.Map<AdministratorInfoDto>(administrator);
            return CreatedAtAction(nameof(AdministratorInfo), null, returnDto);
        }
    }
}