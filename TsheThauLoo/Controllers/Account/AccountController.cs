using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Account;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public AccountController(
            ILogger<AccountController> logger, 
            UserManager<ApplicationUser> userManager, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _userManager = userManager;
            _dbContext = dbContext;
            _mapper = mapper;
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
        [HttpGet("photo", Name = nameof(Photo))]
        public async Task<IActionResult> Photo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.UserPhotos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost("photo", Name = nameof(CreatePhoto))]
        public async Task<IActionResult> CreatePhoto([FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var user = await _dbContext.Users
                    .Include(x => x.UserPhoto)
                    .SingleOrDefaultAsync(x => x.Id == userId);
                if (user.UserPhoto != null)
                {
                    return Problem(title: "禁止修改", detail: "使用者照片已存在", statusCode: 403);
                }
                
                var entity = _mapper.Map(dto, user.UserPhoto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        user.UserPhoto = entity;
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

                var returnDto = _mapper.Map<FileDto>(user.UserPhoto);
                return CreatedAtAction(nameof(Photo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize]
        [HttpDelete("photo", Name = nameof(DeletePhoto))]
        public async Task<IActionResult> DeletePhoto()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.UserPhotos
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.UserPhotos.Remove(entity);
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