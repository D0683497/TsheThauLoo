using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account.Password;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Password;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/account/password")]
    public class PasswordController : ControllerBase
    {
        private readonly ILogger<PasswordController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public PasswordController(
            ILogger<PasswordController> logger, 
            UserManager<ApplicationUser> userManager, 
            TsheThauLooDbContext dbContext, 
            IConfiguration configuration, 
            IMailService mailService)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _configuration = configuration;
            _mailService = mailService;
        }

        [AuthAuthorize]
        [HttpPost(Name = nameof(ChangePassword))]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            ChangePasswordDtoValidator validator = new ChangePasswordDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _userManager.FindByIdAsync(userId);
                
                #region 驗證密碼

                if (!await _userManager.CheckPasswordAsync(user, dto.CurrentPassword))
                {
                    result.Errors.Add(new ValidationFailure("currentPassword", "目前密碼錯誤"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldSecurityStamp = user.SecurityStamp;
                        
                        if (await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }

                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateException)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                return NoContent();
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpPost("forget", Name = nameof(ForgetPassword))]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordDto dto)
        {
            ForgetPasswordDtoValidator validator = new ForgetPasswordDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(dto.Email);
                if (user == null)
                {
                    result.Errors.Add(new ValidationFailure("email", "此電子郵件尚未被註冊"));
                    return BadRequest(result.Errors);
                }
                
                #region UpdateSecurity

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldSecurityStamp = user.SecurityStamp;
                        if (await _userManager.UpdateSecurityStampAsync(user) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        
                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateException)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                #endregion

                #region 寄信

                var link = $"{_configuration["FrontendUrl"]}/account/password/reset" +
                           $"?userId={Uri.EscapeDataString(user.Id)}" +
                           $"&token={Uri.EscapeDataString(await _userManager.GeneratePasswordResetTokenAsync(user))}";

                await _mailService.SendResetPasswordAsync(user.Email, user.Email, link);

                #endregion
                
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpPost("reset", Name = nameof(ResetPassword))]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto dto)
        {
            ResetPasswordDtoValidator validator = new ResetPasswordDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var user = await _userManager.FindByIdAsync(dto.UserId);
                if (user == null)
                {
                    return NotFound();
                }

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldSecurityStamp = user.SecurityStamp;
                        if (await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        await transaction.CommitAsync();
                    }
                    catch (DbUpdateException)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }

                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}