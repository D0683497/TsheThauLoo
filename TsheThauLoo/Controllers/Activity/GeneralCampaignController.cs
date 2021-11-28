using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity;
using TsheThauLoo.Dtos.Activity.GeneralCampaign;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Enums;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Activity.GeneralCampaign;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/campaigns/{campaignId}/general")]
    public class GeneralCampaignController : ControllerBase
    {
        private readonly ILogger<GeneralCampaignController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        private readonly IConfiguration _configuration;

        public GeneralCampaignController(
            ILogger<GeneralCampaignController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper, 
            IMailService mailService, 
            IConfiguration configuration)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _mailService = mailService;
            _configuration = configuration;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetGenerals))]
        public async Task<ActionResult<IEnumerable<GeneralCampaignDto>>> GetGenerals([FromRoute] string campaignId)
        {
            var entities = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .OrderBy(x => x.StartTime)
                .Include(x => x.GeneralCampaignFiles)
                .Include(x => x.Company)
                .ThenInclude(x => x.CompanyLogo)
                .Include(x => x.Company)
                .ThenInclude(x => x.IndustrialClassifications)
                .Where(x => x.CampaignId == campaignId)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<GeneralCampaignDto>>(entities);
            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(CreateGeneral))]
        public async Task<IActionResult> CreateGeneral([FromRoute] string campaignId, [FromBody] GeneralCampaignCreateDto dto)
        {
            var act = await _dbContext.Campaigns
                .Include(x => x.GeneralCampaigns)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
            if (act == null)
            {
                return NotFound();
            }
            
            GeneralCampaignCreateDtoValidator validator = new GeneralCampaignCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                #region 驗證
                
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }

                if (dto.RegistrationStartDate != null)
                {
                    if (CompareDate((DateTime) dto.RegistrationStartDate, act.StartTime) == TimeComparisonStatus.Earlier)
                    {
                        result.Errors.Add(new ValidationFailure("registrationStartDate", $"報名開始日期必須晚於{act.StartTime.ToString("yyyy-MM-dd")}"));
                        return BadRequest(result.Errors);
                    }
                }
                if (CompareDate(dto.StartDate, act.StartTime) == TimeComparisonStatus.Earlier)
                {
                    result.Errors.Add(new ValidationFailure("startDate", $"開始日期必須晚於{act.StartTime.ToString("yyyy-MM-dd")}"));
                    return BadRequest(result.Errors);
                }
                if (CompareDate(act.EndTime, dto.EndDate) == TimeComparisonStatus.Earlier)
                {
                    result.Errors.Add(new ValidationFailure("endDate", $"結束日期必須早於{act.EndTime.ToString("yyyy-MM-dd")}"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var entity = _mapper.Map<GeneralCampaign>(dto);
                act.GeneralCampaigns.Add(entity);
                _dbContext.Campaigns.Update(act);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = entity.CampaignId, generalId = entity.GeneralCampaignId};
                var returnDto = _mapper.Map<GeneralCampaignDto>(entity);
                return CreatedAtAction(nameof(GetGeneral), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{generalId}", Name = nameof(GetGeneral))]
        public async Task<ActionResult<GeneralCampaignDto>> GetGeneral([FromRoute] string campaignId, [FromRoute] string generalId)
        {
            var entity = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .Include(x => x.GeneralCampaignFiles)
                .Include(x => x.Company)
                .ThenInclude(x => x.CompanyLogo)
                .Include(x => x.Company)
                .ThenInclude(x => x.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<GeneralCampaignDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{generalId}", Name = nameof(EditGeneral))]
        public async Task<ActionResult<GeneralCampaignDto>> EditGeneral([FromRoute] string campaignId, [FromRoute] string generalId, [FromBody] GeneralCampaignEditDto dto)
        {
            GeneralCampaignEditDtoValidator validator = new GeneralCampaignEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                var entity = await _dbContext.GeneralCampaigns
                    .Include(x => x.Campaign)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
                if (entity == null)
                {
                    return NotFound();
                }
                
                #region 驗證
                
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
                }
                
                if (dto.RegistrationStartDate != null)
                {
                    if (CompareDate((DateTime) dto.RegistrationStartDate, entity.Campaign.StartTime) == TimeComparisonStatus.Earlier)
                    {
                        result.Errors.Add(new ValidationFailure("registrationStartDate", $"報名開始日期必須晚於{entity.Campaign.StartTime.ToString("yyyy-MM-dd")}"));
                        return BadRequest(result.Errors);
                    }
                }
                if (CompareDate(dto.StartDate, entity.Campaign.StartTime) == TimeComparisonStatus.Earlier)
                {
                    result.Errors.Add(new ValidationFailure("startDate", $"開始日期必須晚於{entity.Campaign.StartTime.ToString("yyyy-MM-dd")}"));
                    return BadRequest(result.Errors);
                }
                if (CompareDate(entity.Campaign.EndTime, dto.EndDate) == TimeComparisonStatus.Earlier)
                {
                    result.Errors.Add(new ValidationFailure("endDate", $"結束日期必須早於{entity.Campaign.EndTime.ToString("yyyy-MM-dd")}"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.GeneralCampaigns.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = entity.CampaignId, generalId = entity.GeneralCampaignId};
                var returnDto = _mapper.Map<GeneralCampaignDto>(entity);
                return CreatedAtAction(nameof(GetGeneral), routeValues, returnDto);
                
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{generalId}", Name = nameof(DeleteGeneral))]
        public async Task<IActionResult> DeleteGeneral([FromRoute] string campaignId, [FromRoute] string generalId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var administrator = await _dbContext.Administrators
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            
            #region 驗證
                
            if (!administrator.AdministratorConfirmed)
            {
                return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
            }

            #endregion
            
            var entity = await _dbContext.GeneralCampaigns
                .Include(x => x.GeneralCampaignFiles)
                .Include(x => x.GeneralCampaignAttendees)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (entity == null)
            {
                return NotFound();
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var users = new List<string>();
                    foreach (var attendee in entity.GeneralCampaignAttendees)
                    {
                        var user = await _dbContext.Users
                            .AsNoTracking()
                            .Select(x => new {x.Id, x.Email})
                            .SingleOrDefaultAsync(x => x.Id == attendee.ApplicationUserId);
                        users.Add(user.Email);
                        _dbContext.GeneralCampaignAttendees.Remove(attendee);
                    }

                    foreach (var file in entity.GeneralCampaignFiles)
                    {
                        _dbContext.GeneralCampaignFiles.Remove(file);
                        await _dbContext.SaveChangesAsync();
                        System.IO.File.Delete(file.Path);
                    }

                    _dbContext.GeneralCampaigns.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    
                    await _mailService.SendActivityDeleteAsync(entity.Title, users);

                    await transaction.CommitAsync();
                }
                catch (IOException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (DbUpdateException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
                catch (SmtpCommandException)
                {
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return NoContent();
        }
        
        [AuthAuthorize]
        [HttpPost("{generalId}/sign-up", Name = nameof(SignUpGeneral))]
        public async Task<IActionResult> SignUpGeneral([FromRoute] string campaignId, [FromRoute] string generalId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
            var act = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);

            #region 驗證

            if (act.EnableIdentityConfirmed && !user.IdentityConfirmed)
            {
                return Problem(title: "報名失敗", detail: "需要實名驗證", statusCode: 403);
            }
            if (act.LimitNumberOfPeople != 0)
            {
                var count = await _dbContext.GeneralCampaignAttendees
                    .Where(x => x.Status == AttendeeStatusType.SignUpSuccess)
                    .Where(x => x.GeneralCampaignId == generalId)
                    .CountAsync();
                if (count >= act.LimitNumberOfPeople)
                {
                    return Problem(title: "報名失敗", detail: "已達人數上限", statusCode: 403);
                }
            }
            var now = DateTime.Now;
            if (CompareDate(now, act.EndTime) != TimeComparisonStatus.Earlier)
            {
                return Problem(title: "報名失敗", detail: "活動已結束", statusCode: 403);
            }
            if (CompareDate(now, act.StartTime) != TimeComparisonStatus.Earlier)
            {
                return Problem(title: "報名失敗", detail: "活動已開始", statusCode: 403);
            }
            if (act.RegistrationStartTime != null)
            {
                if (CompareDate((DateTime) act.RegistrationStartTime, now) != TimeComparisonStatus.Earlier)
                {
                    return Problem(title: "報名失敗", detail: "尚未開始報名", statusCode: 403);
                }
            }
            if (act.RegistrationEndTime != null)
            {
                if (CompareDate((DateTime) act.RegistrationEndTime, now) != TimeComparisonStatus.Later)
                {
                    return Problem(title: "報名失敗", detail: "已結束報名", statusCode: 403);
                }
            }
            if (await _dbContext.GeneralCampaignAttendees.AnyAsync(x => x.GeneralCampaignId == generalId && x.ApplicationUserId == userId))
            {
                return Problem(title: "報名失敗", detail: "已報名", statusCode: 403);
            }

            #endregion

            var entity = new GeneralCampaignAttendee
            {
                Status = act.EnableVerify ? AttendeeStatusType.UnderReview : AttendeeStatusType.SignUpSuccess,
                GeneralCampaignId = generalId,
                ApplicationUserId = userId
            };
            _dbContext.GeneralCampaignAttendees.Add(entity);
            await _dbContext.SaveChangesAsync();
            
            #region 寄信

            var link = $"{_configuration["FrontendUrl"]}/act/campaign/{campaignId}/general/{generalId}";

            await _mailService.SendActivityAttendeeAsync(user.Email, user.Email, link, act.Title, entity.Status);

            #endregion

            return NoContent();
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{generalId}/sign-in", Name = nameof(SignInGeneral))]
        public async Task<IActionResult> SignInGeneral([FromRoute] string campaignId, [FromRoute] string generalId, [FromBody] ActivitySignInDto dto)
        {
            var act = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (act == null)
            {
                return NotFound();
            }
            
            var now = DateTime.Now;
            if (CompareDate(now, act.StartTime) != TimeComparisonStatus.Later)
            {
                return Problem(title: "簽到失敗", detail: "活動尚未開始", statusCode: 403);
            }
            if (CompareDate(now, act.EndTime) != TimeComparisonStatus.Earlier)
            {
                return Problem(title: "簽到失敗", detail: "活動已結束", statusCode: 403);
            }

            var user = await _dbContext.Users
                .AnyAsync(x => x.Id == dto.UserId);
            if (!user)
            {
                return Problem(title: "簽到失敗", detail: "無此使用者", statusCode: 403);
            }

            var attendee = await _dbContext.GeneralCampaignAttendees
                .SingleOrDefaultAsync(x => x.GeneralCampaignId == generalId && x.ApplicationUserId == dto.UserId);
            if (attendee == null)
            {
                return Problem(title: "簽到失敗", detail: "使用者尚未報名", statusCode: 403);
            }

            attendee.Status = AttendeeStatusType.SignInSuccess;
            _dbContext.GeneralCampaignAttendees.Update(attendee);
            await _dbContext.SaveChangesAsync();
            
            return NoContent();
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{generalId}/participate", Name = nameof(ParticipateGeneral))]
        public async Task<IActionResult> ParticipateGeneral([FromRoute] string campaignId, [FromRoute] string generalId, [FromBody] GeneralParticipantDto dto)
        {
            GeneralParticipantDtoValidator validator = new GeneralParticipantDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            
            if (result.IsValid)
            {
                var act = await _dbContext.GeneralCampaigns
                    .Include(x => x.GeneralCampaignParticipants)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
                if (act == null)
                {
                    return NotFound();
                }
                
                var now = DateTime.Now;
                if (CompareDate(now, act.StartTime) != TimeComparisonStatus.Later)
                {
                    return Problem(title: "簽到失敗", detail: "活動尚未開始", statusCode: 403);
                }
                if (CompareDate(now, act.EndTime) != TimeComparisonStatus.Earlier)
                {
                    return Problem(title: "簽到失敗", detail: "活動已結束", statusCode: 403);
                }

                var entity = _mapper.Map<GeneralCampaignParticipant>(dto);
                act.GeneralCampaignParticipants.Add(entity);
                _dbContext.GeneralCampaigns.Update(act);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }

            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{generalId}/invite", Name = nameof(InviteGeneral))]
        public async Task<IActionResult> InviteGeneral([FromRoute] string campaignId, [FromRoute] string generalId, [FromBody] CompanyInviteDto dto)
        {
            var act = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (act == null)
            {
                return NotFound();
            }

            var company = await _dbContext.Companies
                .AsNoTracking()
                .Include(x => x.CompanyLogo)
                .Include(x => x.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.CompanyId == dto.CompanyId);
            if (company == null)
            {
                return NotFound();
            }
            
            act.CompanyId = dto.CompanyId;
            _dbContext.GeneralCampaigns.Update(act);
            await _dbContext.SaveChangesAsync();

            var returnDto = _mapper.Map<CompanyDto>(company);
            return Ok(returnDto);
        }
        
        private TimeComparisonStatus CompareDate(DateTime firstDate, DateTime secondDate)
        {
            var result = (TimeComparisonStatus) DateTime.Compare(firstDate, secondDate);
            return result;
        }
    }
}