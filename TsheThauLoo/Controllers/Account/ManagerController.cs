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
using TsheThauLoo.Dtos.Account.Profile.Manager;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Account.Profile;
using TsheThauLoo.Validator.Account.Register;

namespace TsheThauLoo.Controllers.Account
{
    [ApiController]
    [AuthAuthorize(Roles = "Manager")]
    [Route("api/account/manager")]
    public class ManagerController : ControllerBase
    {
        private readonly ILogger<ManagerController> _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public ManagerController(
            ILogger<ManagerController> logger, 
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
        [HttpPost("register", Name = nameof(ManagerRegister))]
        public async Task<IActionResult> ManagerRegister([FromBody] ManagerRegisterDto dto)
        {
            ManagerRegisterDtoValidator validator = new ManagerRegisterDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                #region ????????????

                if (await _userManager.Users.AnyAsync(x => x.UserName == dto.UserName))
                {
                    result.Errors.Add(new ValidationFailure("userName", "??????????????????????????????"));
                }
                if (await _userManager.Users.AnyAsync(x => x.Email == dto.Email))
                {
                    result.Errors.Add(new ValidationFailure("email", "???????????????????????????"));
                }
                if (!string.IsNullOrEmpty(dto.PhoneNumber))
                {
                    if (await _userManager.Users.AnyAsync(x => x.PhoneNumber == dto.PhoneNumber))
                    {
                        result.Errors.Add(new ValidationFailure("phoneNumber", "???????????????????????????"));
                    }
                }
                if (!string.IsNullOrEmpty(dto.NationalId))
                {
                    if (await _userManager.Users.AnyAsync(x => x.NationalId == dto.NationalId.ToUpper()))
                    {
                        result.Errors.Add(new ValidationFailure("nationalId", "??????????????????????????????"));
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
                            #region ???????????????

                            if (await _userManager.CreateAsync(entity, dto.Password) != IdentityResult.Success)
                            {
                                throw new DbUpdateException();
                            }

                            #endregion
                            
                            #region ?????? Claim

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
                            
                            #region ????????????

                            if (await _userManager.AddToRoleAsync(entity, "Manager") != IdentityResult.Success)
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
                    
                    #region ??????

                    var link = $"{_configuration["FrontendUrl"]}/account/email/confirm" + 
                               $"?userId={Uri.EscapeDataString(entity.Id)}" + 
                               $"&token={Uri.EscapeDataString(await _userManager.GenerateEmailConfirmationTokenAsync(entity))}";

                    await _mailService.SendEmailConfirmAsync(entity.Email, entity.Email, link, true);

                    #endregion

                    var returnDto = _mapper.Map<ManagerProfileDto>(entity);
                    return CreatedAtAction(nameof(ManagerProfile), null, returnDto);
                }
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpGet("profile", Name = nameof(ManagerProfile))]
        public async Task<ActionResult<ManagerProfileDto>> ManagerProfile()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Users
                .AsNoTracking()
                .Include(x => x.UserPhoto)
                .Include(x => x.Manager)
                .Include(x => x.Manager.Substitute)
                .SingleOrDefaultAsync(x => x.Id == userId);
            var dto = _mapper.Map<ManagerProfileDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpGet("profile/info", Name = nameof(ManagerInfo))]
        public async Task<ActionResult<ManagerInfoDto>> ManagerInfo()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Managers
                .AsNoTracking()
                .Include(x => x.Substitute)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            var dto = _mapper.Map<ManagerInfoDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpPost("profile/info", Name = nameof(ManagerEditInfo))]
        public async Task<ActionResult<ManagerInfoDto>> ManagerEditInfo([FromBody] ManagerEditInfoDto dto)
        {
            ManagerEditInfoDtoValidator validator = new ManagerEditInfoDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var entity = await _dbContext.Managers
                    .Include(x => x.Substitute)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (entity.ManagerConfirmed)
                {
                    return Problem(title: "????????????", detail: "????????????????????????", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Managers.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<ManagerInfoDto>(updateEntity);
                return CreatedAtAction(nameof(ManagerInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpPost("profile/info/substitute", Name = nameof(ManagerEditSubstitute))]
        public async Task<ActionResult<ManagerInfoDto>> ManagerEditSubstitute([FromBody] SubstituteEditDto dto)
        {
            SubstituteEditDtoValidator validator = new SubstituteEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var manager = await _dbContext.Managers
                    .Include(x => x.Substitute)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                if (manager.ManagerConfirmed)
                {
                    return Problem(title: "????????????", detail: "????????????????????????", statusCode: 403);
                }
                var updateEntity = _mapper.Map(dto, manager.Substitute);
                _dbContext.Substitutes.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var returnDto = _mapper.Map<ManagerInfoDto>(manager);
                return CreatedAtAction(nameof(ManagerInfo), null, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}