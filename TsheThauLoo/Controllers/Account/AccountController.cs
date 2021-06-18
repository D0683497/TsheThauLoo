using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
using TsheThauLoo.Dtos.Account.National;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account;
using TsheThauLoo.Validator.Account.File;
using TsheThauLoo.Validator.Account.National;

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
        private readonly IMapper _mapper;

        public AccountController(
            ILogger<AccountController> logger, 
            UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, 
            RoleManager<ApplicationRole> roleManager, 
            IConfiguration configuration, 
            IWebHostEnvironment environment, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _environment = environment;
            _dbContext = dbContext;
            _mapper = mapper;
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

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var oldUserName = user.UserName;
                        if (await _userManager.ReplaceClaimAsync(user, new Claim(ClaimTypes.Name, oldUserName), new Claim(ClaimTypes.Name, dto.NewUserName)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        user.UserName = dto.NewUserName;
                        user.NormalizedUserName = dto.NewUserName.ToUpper();
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

                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [AuthAuthorize]
        [HttpPost("phone", Name = nameof(ChangePhone))]
        public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneDto dto)
        {
            ChangePhoneDtoValidator validator = new ChangePhoneDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region 驗證重複

                if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == dto.NewPhoneNumber))
                {
                    result.Errors.Add(new ValidationFailure("newPhoneNumber", "新的手機號碼已經被使用"));
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
                        user.PhoneNumber = dto.NewPhoneNumber;
                        user.PhoneNumberConfirmed = false;
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

                return NoContent();
            }
            return BadRequest(result.Errors);
        }

        [AuthAuthorize]
        [HttpGet("national", Name = nameof(National))]
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
        [HttpPost("national", Name = nameof(EditNational))]
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
                            if (await _userManager.Users.AnyAsync(x => x.NationalId == dto.NationalId.ToUpper()))
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
        [HttpGet("national/verify", Name = nameof(NationalVerify))]
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
        [HttpPost("national/verify", Name = nameof(EditNationalVerify))]
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
        [HttpPost("national/verify/files", Name = nameof(CreateNationalVerifyFile))]
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
        [HttpPost("national/verify/files/{fileId}", Name = nameof(EditNationalVerifyFile))]
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
        [HttpGet("national/verify/files/{fileId}", Name = nameof(NationalVerifyFile))]
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
        [HttpGet("national/verify/files/{fileId}/download", Name = nameof(DownloadNationalVerifyFile))]
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
        [HttpDelete("national/verify/files/{fileId}", Name = nameof(DeleteNationalVerifyFile))]
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