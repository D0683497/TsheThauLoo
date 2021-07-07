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
using TsheThauLoo.Dtos.Account.Profile.Examiner;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Profile;
using TsheThauLoo.Validator.Account.Register;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize(Roles = "Examiner")]
    [Route("api/account/examiner")]
    public class ExaminerController : ControllerBase
    {
        private readonly ILogger<ExaminerController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public ExaminerController(
            ILogger<ExaminerController> logger, 
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
        [HttpPost("register", Name = nameof(ExaminerRegister))]
        public async Task<IActionResult> ExaminerRegister([FromBody] ExaminerRegisterDto dto)
        {
            ExaminerRegisterDtoValidator validator = new ExaminerRegisterDtoValidator();
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

                            if (await _userManager.AddToRoleAsync(entity, "Examiner") != IdentityResult.Success)
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

                    var returnDto = _mapper.Map<ExaminerProfileDto>(entity);
                    return CreatedAtAction(nameof(ExaminerProfile), null, returnDto);
                }
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Examiner")]
        [HttpGet("profile", Name = nameof(ExaminerProfile))]
        public async Task<ActionResult<ExaminerProfileDto>> ExaminerProfile()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.UserPhoto)
                .Include(x => x.Examiner)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<ExaminerProfileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Examiner")]
        [HttpGet("profile/info", Name = nameof(ExaminerInfo))]
        public async Task<ActionResult<ExaminerInfoDto>> ExaminerInfo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Examiners
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<ExaminerInfoDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Examiner")]
        [HttpPost("profile/info", Name = nameof(ExaminerEditInfo))]
        public async Task<ActionResult<ExaminerInfoDto>> ExaminerEditInfo([FromBody] ExaminerEditInfoDto dto)
        {
            ExaminerEditInfoDtoValidator validator = new ExaminerEditInfoDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Examiners
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (entity.ExaminerConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "審查員已驗證", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Examiners.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<ExaminerInfoDto>(updateEntity);
                return CreatedAtAction(nameof(ExaminerInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}