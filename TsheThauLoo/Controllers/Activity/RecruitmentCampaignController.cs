using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Enums;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Activity.RecruitmentCampaign;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/campaigns/{campaignId}/recruitment")]
    public class RecruitmentCampaignController : ControllerBase
    {
        private readonly ILogger<RecruitmentCampaignController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public RecruitmentCampaignController(
            ILogger<RecruitmentCampaignController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetRecruitments))]
        public async Task<ActionResult<IEnumerable<RecruitmentCampaignDto>>> GetRecruitments([FromRoute] string campaignId)
        {
            var entities = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .OrderBy(x => x.StartTime)
                .Include(x => x.RecruitmentCampaignFiles)
                .Include(x => x.Company)
                .ThenInclude(x => x.CompanyLogo)
                .Include(x => x.Company)
                .ThenInclude(x => x.IndustrialClassifications)
                .Where(x => x.CampaignId == campaignId)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<RecruitmentCampaignDto>>(entities);
            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(CreateRecruitment))]
        public async Task<IActionResult> CreateRecruitment([FromRoute] string campaignId, [FromBody] RecruitmentCampaignCreateDto dto)
        {
            var act = await _dbContext.Campaigns
                .Include(x => x.RecruitmentCampaigns)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
            if (act == null)
            {
                return NotFound();
            }
            
            RecruitmentCampaignCreateDtoValidator validator = new RecruitmentCampaignCreateDtoValidator();
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
                
                var entity = _mapper.Map<RecruitmentCampaign>(dto);
                act.RecruitmentCampaigns.Add(entity);
                _dbContext.Campaigns.Update(act);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = entity.CampaignId, recruitmentId = entity.RecruitmentCampaignId};
                var returnDto = _mapper.Map<RecruitmentCampaignDto>(entity);
                return CreatedAtAction(nameof(GetRecruitment), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{recruitmentId}", Name = nameof(GetRecruitment))]
        public async Task<ActionResult<RecruitmentCampaignDto>> GetRecruitment([FromRoute] string campaignId, [FromRoute] string recruitmentId)
        {
            var entity = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignFiles)
                .Include(x => x.Company)
                .ThenInclude(x => x.CompanyLogo)
                .Include(x => x.Company)
                .ThenInclude(x => x.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<RecruitmentCampaignDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{recruitmentId}", Name = nameof(EditRecruitment))]
        public async Task<ActionResult<RecruitmentCampaignDto>> EditRecruitment([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromBody] RecruitmentCampaignEditDto dto)
        {
            RecruitmentCampaignEditDtoValidator validator = new RecruitmentCampaignEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var administrator = await _dbContext.Administrators
                    .AsNoTracking()
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                var entity = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.Campaign)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                if (entity == null)
                {
                    return NotFound();
                }
                
                #region 驗證
                
                if (!administrator.AdministratorConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "管理員尚未驗證", statusCode: 403);
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
                _dbContext.RecruitmentCampaigns.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = entity.CampaignId, recruitmentId = entity.RecruitmentCampaignId};
                var returnDto = _mapper.Map<RecruitmentCampaignDto>(entity);
                return CreatedAtAction(nameof(GetRecruitment), routeValues, returnDto);
                
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{recruitmentId}", Name = nameof(DeleteRecruitment))]
        public async Task<IActionResult> DeleteRecruitment([FromRoute] string campaignId, [FromRoute] string recruitmentId)
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
            
            var entity = await _dbContext.RecruitmentCampaigns
                .Include(x => x.RecruitmentCampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            if (entity == null)
            {
                return NotFound();
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    // TODO: RecruitmentCampaignOpening
                    
                    foreach (var file in entity.RecruitmentCampaignFiles)
                    {
                        _dbContext.RecruitmentCampaignFiles.Remove(file);
                        await _dbContext.SaveChangesAsync();
                        System.IO.File.Delete(file.Path);
                    }

                    _dbContext.RecruitmentCampaigns.Remove(entity);
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
                    await transaction.RollbackAsync();
                    throw;
                }
            }

            return NoContent();
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{recruitmentId}/invite", Name = nameof(InviteRecruitment))]
        public async Task<IActionResult> InviteRecruitment([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromBody] CompanyInviteDto dto)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
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
            _dbContext.RecruitmentCampaigns.Update(act);
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