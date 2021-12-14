using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data;
using TsheThauLoo.Entities.Identity;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Models.Account;
using TsheThauLoo.Services;
using TsheThauLoo.Utils;

namespace TsheThauLoo.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly ILogger<AccountController> _logger;
    private readonly IMapper _mapper;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly TsheThauLooDbContext _dbContext;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AccountController(
        ILogger<AccountController> logger, 
        IMapper mapper, 
        UserManager<ApplicationUser> userManager, 
        TsheThauLooDbContext dbContext, 
        SignInManager<ApplicationUser> signInManager, 
        ITokenService tokenService)
    {
        _logger = logger;
        _mapper = mapper;
        _userManager = userManager;
        _dbContext = dbContext;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<TokenDto>> Login([FromBody] LoginDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.UserName);
        if (user == null)
        {
            return Problem(title: "登入失敗", detail: "請檢查您的帳號密碼是否正確", statusCode: 401);
        }
        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, true);
        if (result.Succeeded)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new (JwtClaims.Subject, user.Id));
            claims.Add(new (JwtClaims.Username, user.UserName));
            claims.Add(new (JwtClaims.Email, user.Email));
            claims.Add(new (JwtClaims.EmailVerified, user.EmailConfirmed.ToString().ToLowerInvariant()));
            claims.Add(new (JwtClaims.FullName, user.Name));
            foreach (var role in roles)
            {
                claims.Add(new (JwtClaims.Roles, role));
            }
            var token = _tokenService.GenerateJwtToken(claims);
            var returnDto = new TokenDto(token);
            return Ok(returnDto);
        }
        if (result.IsLockedOut)
        {
            return Problem(title: "登入失敗", detail: "帳戶鎖定中，請聯絡管理員", statusCode: 403);
        }
        if (result.IsNotAllowed)
        {
            return Problem(title: "登入失敗", detail: "帳戶尚未驗證，請前往您的信箱收取驗證信", statusCode: 403);
        }
        if (result.RequiresTwoFactor)
        {
            return Problem(title: "需要二階段驗證", statusCode: 200);
        }
        return Problem(title: "登入失敗", detail: "請檢查您的帳號密碼是否正確", statusCode: 401);
    }
    
    // TODO: Logout
    
    [AllowAnonymous]
    [HttpPost("register/alumnus")]
    public async Task<IActionResult> AlumnusRegister([FromBody] AlumnusRegisterDto dto)
    {
        var user = _mapper.Map<AlumnusRegisterDto, ApplicationUser>(dto);
        await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddClaimAsync(user, new (JwtClaims.Groups, $"{user.Alumnus!.College}-{user.Alumnus!.Department}"));
                await _userManager.AddToRoleAsync(user, "Alumnus");
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return Problem(title: "註冊失敗", detail: "資料庫連線異常", statusCode: 500);
            }
        }
        var returnDto = _mapper.Map<ApplicationUser, ProfileDto>(user);
        return CreatedAtAction(nameof(Profile), returnDto);
    }
    
    [AllowAnonymous]
    [HttpPost("register/employee")]
    public async Task<IActionResult> EmployeeRegister([FromBody] EmployeeRegisterDto dto)
    {
        var user = _mapper.Map<EmployeeRegisterDto, ApplicationUser>(dto);
        await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
        {
            try
            {
                var result = await _userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                await _userManager.AddClaimAsync(user, new (JwtClaims.Groups, $"{user.Alumnus!.College}-{user.Alumnus!.Department}"));
                await _userManager.AddToRoleAsync(user, "Alumnus");
                await transaction.CommitAsync();
            }
            catch (DbUpdateException)
            {
                await transaction.RollbackAsync();
                return Problem(title: "註冊失敗", detail: "資料庫連線異常", statusCode: 500);
            }
        }
        var returnDto = _mapper.Map<ApplicationUser, ProfileDto>(user);
        return CreatedAtAction(nameof(Profile), returnDto);
    }
    
    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ProfileDto>> Profile()
    {
        var entity = await _userManager.GetUserAsync(User);
        if (entity == null)
        {
            return Unauthorized();
        }
        var dto = _mapper.Map<ApplicationUser, ProfileDto>(entity);
        return Ok(dto);
    }
    
    [Authorize(Roles = "Alumnus")]
    [HttpGet("profile/alumnus")]
    public async Task<ActionResult<AlumnusProfileDto>> AlumnusProfile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        var entity = await _dbContext.Alumni
            .AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<Alumnus, AlumnusProfileDto>(entity);
        return Ok(dto);
    }

    [Authorize(Roles = "Employee")]
    [HttpGet("profile/employee")]
    public async Task<ActionResult<EmployeeProfileDto>> EmployeeProfile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        var entity = await _dbContext.Employees
            .AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<Employee, EmployeeProfileDto>(entity);
        return Ok(dto);
    }
    
    [Authorize]
    [HttpGet("profile/staff")]
    public async Task<ActionResult<StaffProfileDto>> StaffProfile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        var entity = await _dbContext.Staffs
            .AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<Staff, StaffProfileDto>(entity);
        return Ok(dto);
    }
    
    [Authorize]
    [HttpGet("profile/student")]
    public async Task<ActionResult<StudentProfileDto>> StudentProfile()
    {
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }
        var entity = await _dbContext.Students
            .AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(x => x.UserId == userId);
        if (entity == null)
        {
            return NotFound();
        }
        var dto = _mapper.Map<Student, StudentProfileDto>(entity);
        return Ok(dto);
    }
    
    // TODO: UpdateProfile
    
    // TODO: DeleteProfile
    
    // TODO: UpdateUserName
    
    // TODO: UpdateEmail
    
    // TODO: ConfirmEmail
    
    // TODO: UpdatePhoneNumber
    
    // TODO: ResetPassword
    
    // TODO: ForgetPassword
}