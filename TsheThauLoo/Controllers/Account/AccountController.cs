using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account;
using TsheThauLoo.Dtos.Account.Login;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly TsheThauLooDbContext _dbContext;

        public AccountController(
            ILogger<AccountController> logger, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> roleManager, 
            IConfiguration configuration, 
            IWebHostEnvironment environment, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _environment = environment;
            _dbContext = dbContext;
        }
        
        [AllowAnonymous]
        [HttpPost("login", Name = nameof(Login))]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            LoginDtoValidator validator = new LoginDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region 檢查是否可登入

                var user = await _userManager.FindByNameAsync(dto.UserName);
                if (user == null)
                {
                    return Problem(title: "登入失敗", detail: "請檢查您的帳號密碼是否正確", statusCode: 403);
                }
                if (!user.EmailConfirmed)
                {
                    return Problem(title: "帳戶尚未驗證", detail: "請前往您的信箱收取驗證信", statusCode: 403);
                }
                if (!user.IsEnable)
                {
                    return Problem(title: "帳戶尚未啟用", detail: "請聯絡管理員", statusCode: 403);
                }

                #endregion
                
                #region 檢查密碼
                
                var checkPasswordResult = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
                if (checkPasswordResult.IsLockedOut)
                {
                    return Problem(title: "帳戶被鎖定", detail: "請聯絡管理員", statusCode: 403);
                }
                if (checkPasswordResult.IsNotAllowed)
                {
                    return Problem(title: "帳戶尚未驗證", detail: "請前往您的信箱收取驗證信", statusCode: 403);
                }
                if (checkPasswordResult.Succeeded)
                {
                    #region 添加角色聲明
                    
                    var claims = await _userManager.GetClaimsAsync(user);
                    var roleNames = await _userManager.GetRolesAsync(user);
                    foreach (var roleName in roleNames)
                    {
                        var role = await _roleManager.FindByNameAsync(roleName);
                        var roleClaims = await _roleManager.GetClaimsAsync(role);
                        foreach (var roleClaim in roleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }

                    #endregion
                    
                    var token = GenerateJwtToken(claims);
                    var returnDto = new LoginResponseDto {AccessToken = token};
                    return Ok(returnDto);
                }    

                #endregion
                
                return Problem(title: "登入失敗", detail: "請檢查您的帳號密碼是否正確", statusCode: 403);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpPost("username", Name = nameof(ChangeUserName))]
        public async Task<IActionResult> ChangeUserName([FromBody] ChangeUserNameDto dto)
        {
            ChangeUserNameDtoValidator validator = new ChangeUserNameDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region 驗證重複

                if (await _userManager.Users.AnyAsync(x => x.UserName == dto.NewUserName))
                {
                    result.Errors.Add(new ValidationFailure("newUserName", "新的使用者名稱已經被使用"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _userManager.FindByIdAsync(userId);
                user.UserName = dto.NewUserName;
                user.NormalizedUserName = dto.NewUserName.ToUpper();
                _dbContext.Users.Update(user);
                await _dbContext.SaveChangesAsync();

                #region UpdateSecurity

                var oldSecurityStamp = user.SecurityStamp;
                await _userManager.UpdateSecurityStampAsync(user);
                await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, user.SecurityStamp));

                #endregion

                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        private string GenerateJwtToken(IList<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            
            var userClaimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var signingCredentials = new SigningCredentials(securityKey, 
                _environment.IsDevelopment() ? SecurityAlgorithms.HmacSha256 : SecurityAlgorithms.HmacSha256Signature);
            
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                Subject = userClaimsIdentity,
                NotBefore = DateTime.UtcNow, // Token 在什麼時間之前，不可用
                IssuedAt = DateTime.UtcNow, // Token 的建立時間
                Expires = DateTime.Now.AddHours(8), // Token 的逾期時間
                SigningCredentials = signingCredentials
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var serializeToken = tokenHandler.WriteToken(securityToken);

            return serializeToken;
        }
    }
}