using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using TsheThauLoo.Dtos.Account.Login;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Validator.Account;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/account")]
    public class LoginController : ControllerBase
    {
        private readonly ILogger<LoginController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IOAuthService _oAuthService;

        public LoginController(
            ILogger<LoginController> logger,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            RoleManager<ApplicationRole> roleManager,
            IConfiguration configuration,
            IWebHostEnvironment environment, 
            IOAuthService oAuthService)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _environment = environment;
            _oAuthService = oAuthService;
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

        [AllowAnonymous]
        [HttpGet("nid-login", Name = nameof(NIDLoginUrl))]
        public IActionResult NIDLoginUrl()
        {
            var url = $"{_configuration["NIDSettings:NIDUrl"]}/fcuOauth/Auth.aspx?" +
                      $"client_id={_configuration["NIDSettings:ClientId"]}&" +
                      $"client_url={_configuration["NIDSettings:ClientUrl"]}";
            return Redirect(url);
        }

        [AllowAnonymous]
        [HttpPost("nid-login", Name = nameof(NIDLogin))]
        public async Task<IActionResult> NIDLogin([FromForm] NIDLoginDto dto)
        {
            _logger.LogInformation($"NID 登入: {dto.Status}, {dto.Message}, {dto.UserCode}");
            
            switch (dto.Status)
            {
                case 100:
                    _logger.LogInformation("NID 登入: 使用者拒絕授權");
                    return Problem(title: "NID 登入失敗", detail: "使用者拒絕授權");
                case 200:
                    var nidUser = await _oAuthService.GetNIDUserInfoAsync(dto.UserCode);
                    var user = await _oAuthService.HandleNIDLoginAsync(nidUser);
                    if (!user.EmailConfirmed)
                    {
                        return Problem(title: "登入失敗", detail: "帳戶尚未驗證", statusCode: 403);
                    }
                    if (!user.IsEnable)
                    {
                        return Problem(title: "登入失敗", detail: "帳戶尚未啟用", statusCode: 403);
                    }
                    
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
                    var url = $"{_configuration["FrontendUrl"]}/account/login/nid?token={token}";
                    return Redirect(url);
                case 300:
                    _logger.LogInformation("NID 登入: 應用程式授權已到期");
                    return Problem(title: "NID 登入失敗", detail: "應用程式授權已到期", statusCode: 403);
                case 400:
                    _logger.LogInformation("NID 登入: 欠缺必要的參數、有不正確的參數、有重複的參數、或其他原因導致無法解讀");
                    return Problem(title: "NID 登入失敗", detail: "欠缺必要的參數、有不正確的參數、有重複的參數、或其他原因導致無法解讀", statusCode: 403);
                case 500:
                    _logger.LogInformation("NID 登入: 認證伺服器因為過載或維修中而暫時無法處理請求");
                    return Problem(title: "NID 登入失敗", detail: "認證伺服器因為過載或維修中而暫時無法處理請求", statusCode: 403);
                default:
                    _logger.LogInformation("NID 登入: 發生例外況狀");
                    return Problem(title: "NID 登入失敗", detail: "發生例外況狀", statusCode: 403);
            }
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