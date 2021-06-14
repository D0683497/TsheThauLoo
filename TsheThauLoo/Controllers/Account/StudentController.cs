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
using MimeKit;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account.Profile.Student;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Profile;
using TsheThauLoo.Validator.Account.Register;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize(Roles = "Student")]
    [Route("api/account/student")]
    public class StudentController : ControllerBase
    {
        private readonly ILogger<StudentController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public StudentController(
            ILogger<StudentController> logger, 
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
        [HttpPost("register", Name = nameof(StudentRegister))]
        public async Task<IActionResult> StudentRegister([FromBody] StudentRegisterDto dto)
        {
            StudentRegisterDtoValidator validator = new StudentRegisterDtoValidator();
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
                    if (await _userManager.Users.AnyAsync(x => x.Student.NetworkId == dto.NetworkId.ToUpper()))
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

                            if (await _userManager.AddToRoleAsync(entity, "Student") != IdentityResult.Success)
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

                    await _mailService.SendLinkEmailAsync(MessageImportance.High, entity.Email, entity.Email, 
                        "用戶電子郵件驗證", 
                        "<p><b>感謝您註冊【逢甲大學-成就人才發展系統】</b></p>" +
                        "<p>請點擊下方按鈕驗證您的電子郵件</p>", 
                        link, "立即驗證",
                        $"<p>若您無法直接點擊連結，請複製以下網址，在瀏覽器網址列中貼上：</p>" +
                        $"<p><a href=\"{link}\">{link}</a></p>");

                    #endregion

                    var returnDto = _mapper.Map<StudentProfileDto>(entity);
                    return CreatedAtAction(nameof(StudentProfile), null, returnDto);
                }
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Student")]
        [HttpGet("profile", Name = nameof(StudentProfile))]
        public async Task<ActionResult<StudentProfileDto>> StudentProfile()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.Student)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<StudentProfileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Student")]
        [HttpGet("profile/info", Name = nameof(StudentInfo))]
        public async Task<ActionResult<StudentInfoDto>> StudentInfo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Students
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<StudentInfoDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Student")]
        [HttpPost("profile/info", Name = nameof(StudentEditInfo))]
        public async Task<ActionResult<StudentInfoDto>> StudentEditInfo([FromBody] StudentEditInfoDto dto)
        {
            StudentEditInfoDtoValidator validator = new StudentEditInfoDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Students
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (entity.StudentConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "在校生已驗證", statusCode: 403);
                }

                #region 驗證重複

                if (entity.NetworkId != dto.NetworkId && !string.IsNullOrEmpty(dto.NetworkId))
                {
                    if (await _userManager.Users.AnyAsync(x => x.Student.NetworkId == dto.NetworkId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("networkId", "證號已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }

                #endregion
                
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Students.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<StudentInfoDto>(updateEntity);
                return CreatedAtAction(nameof(StudentInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}