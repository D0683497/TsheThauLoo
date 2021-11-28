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
using TsheThauLoo.Dtos.Account.Email;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Email;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/account/email")]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public EmailController(
            ILogger<EmailController> logger, 
            UserManager<ApplicationUser> userManager, 
            TsheThauLooDbContext dbContext, 
            IMailService mailService, 
            IConfiguration configuration)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _mailService = mailService;
            _configuration = configuration;
        }

        [AuthAuthorize]
        [HttpPost(Name = nameof(ChangeEmail))]
        public async Task<IActionResult> ChangeEmail([FromBody] ChangeEmailDto dto)
        {
            ChangeEmailDtoValidator validator = new ChangeEmailDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region 驗證重複

                if (await _userManager.Users.AnyAsync(x => x.Email == dto.NewEmail))
                {
                    result.Errors.Add(new ValidationFailure("newEmail", "新的電子郵件已經被使用"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _userManager.FindByIdAsync(userId);

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldEmail = user.Email;
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Email, oldEmail), new Claim(ClaimTypes.Email, dto.NewEmail)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        user.Email = dto.NewEmail;
                        user.NormalizedEmail = dto.NewEmail.ToUpper();
                        user.EmailConfirmed = false;
                        _dbContext.Users.Update(user);
                        if (await _dbContext.SaveChangesAsync() < 0)
                        {
                            throw new DbUpdateException();
                        }
                        
                        #region UpdateSecurity

                        var oldSecurityStamp = user.SecurityStamp;
                        if (await _userManager.UpdateSecurityStampAsync(user) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp)) != IdentityResult.Success)
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
                           $"?userId={Uri.EscapeDataString(user.Id)}" + 
                           $"&token={Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(user))}";

                await _mailService.SendEmailConfirmAsync(user.Email, user.Email, link, false);

                #endregion
                
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpPost("confirm", Name = nameof(ConfirmEmail))]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto dto)
        {
            ConfirmEmailDtoValidator validator = new ConfirmEmailDtoValidator();
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
                        if (await _userManager.ConfirmEmailAsync(user, dto.Token) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        
                        #region UpdateSecurity

                        var oldSecurityStamp = user.SecurityStamp;
                        if (await _userManager.UpdateSecurityStampAsync(user) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp)) != IdentityResult.Success)
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

                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}