using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Manage;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Manage;

namespace TsheThauLoo.Controllers.Manage
{
    [ApiController]
    [AuthAuthorize(Roles = "Administrator")]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {
        private readonly ILogger<StudentsController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(
            ILogger<StudentsController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _userManager = userManager;
        }
        
        [HttpGet(Name = nameof(GetStudents))]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetStudents([FromQuery] PaginationResourceParameters parameters)
        {
            var query = _dbContext.Students
                .AsNoTracking()
                .Include(x => x.ApplicationUser)
                .ThenInclude(x => x.UserPhoto);
            
            var entities = await query
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<StudentDto>>(entities);
            
            #region 分頁資訊

            var length = await query.CountAsync();
            var paginationMetadata = new
            {
                pageLength = length, // 總資料數
                pageSize = parameters.PageSize, // 一頁的項目數
                pageIndex = parameters.PageIndex, // 目前頁碼
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            #endregion
            
            return Ok(dtos);
        }
        
        [HttpGet("{userId}", Name = nameof(GetStudent))]
        public async Task<ActionResult<StudentDto>> GetStudent([FromRoute] string userId)
        {
            var entity = await _dbContext.Students
                .AsNoTracking()
                .Include(x => x.ApplicationUser)
                .ThenInclude(x => x.UserPhoto)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (entity == null)
            {
                return NotFound();
            }

            var dto = _mapper.Map<StudentDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{userId}", Name = nameof(EditStudent))]
        public async Task<IActionResult> EditStudent([FromRoute] string userId, [FromBody] StudentManageDto dto)
        {
            StudentManageDtoValidator validator = new StudentManageDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var entity = await _dbContext.Students
                    .Include(x => x.ApplicationUser)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);

                #region 驗證重複

                if (entity.ApplicationUser.UserName != dto.UserName)
                {
                    if (await _dbContext.Users.AnyAsync(x => x.UserName == dto.UserName))
                    {
                        result.Errors.Add(new ValidationFailure("userName", "使用者名稱已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }

                if (entity.ApplicationUser.Email != dto.Email)
                {
                    if (await _dbContext.Users.AnyAsync(x => x.Email == dto.Email))
                    {
                        result.Errors.Add(new ValidationFailure("email", "電子郵件已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }
                
                if (entity.ApplicationUser.PhoneNumber != dto.PhoneNumber && !string.IsNullOrEmpty(dto.PhoneNumber))
                {
                    if (await _dbContext.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber))
                    {
                        result.Errors.Add(new ValidationFailure("phoneNumber", "手機號碼已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }
                
                if (entity.ApplicationUser.NationalId != dto.NationalId && !string.IsNullOrEmpty(dto.NationalId))
                {
                    if (await _dbContext.Users.AnyAsync(x => x.NationalId == dto.NationalId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("nationalId", "身份證字號已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }
                
                if (entity.NetworkId != dto.NetworkId)
                {
                    if (await _dbContext.Administrators.AnyAsync(x => x.NetworkId == dto.NetworkId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("networkId", "證號已經被使用"));
                        return BadRequest(result.Errors);
                    }
                }

                #endregion
                
                var updateEntity = _mapper.Map(dto, entity);
                
                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        if (entity.ApplicationUser.UserName != dto.UserName)
                        {
                            if (await _userManager.ReplaceClaimAsync(entity.ApplicationUser, new Claim(ClaimTypes.Name, entity.ApplicationUser.UserName), new Claim(ClaimTypes.Name, dto.UserName)) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }
                        }
                        
                        if (entity.ApplicationUser.Email != dto.Email)
                        {
                            if (await _userManager.ReplaceClaimAsync(entity.ApplicationUser, new Claim(ClaimTypes.Email, entity.ApplicationUser.Email), new Claim(ClaimTypes.Email, dto.Email)) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }
                        }

                        #region UpdateSecurity

                        var oldSecurityStamp = entity.ApplicationUser.SecurityStamp;
                        if (await _userManager.UpdateSecurityStampAsync(entity.ApplicationUser) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }
                        if (await _userManager.ReplaceClaimAsync(entity.ApplicationUser, new Claim(ClaimTypes.Sid, oldSecurityStamp), new Claim(ClaimTypes.Sid, entity.ApplicationUser.SecurityStamp)) != IdentityResult.Success)
                        {
                            throw new DbUpdateException();
                        }

                        #endregion
                        
                        _dbContext.Students.Update(updateEntity);
                        if (await _dbContext.SaveChangesAsync() < 0)
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
                
                var routeValues = new {userId = entity.ApplicationUserId};
                var returnDto = _mapper.Map<StudentDto>(updateEntity);
                return CreatedAtAction(nameof(GetStudent), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}