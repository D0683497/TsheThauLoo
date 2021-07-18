using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.Campaign;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Enums;
using TsheThauLoo.Parameters;
using TsheThauLoo.Services.Interface;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Activity.Campaign;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/campaigns")]
    public class CampaignController : ControllerBase
    {
        private readonly ILogger<CampaignController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;

        public CampaignController(
            ILogger<CampaignController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper, 
            IMailService mailService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _mailService = mailService;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetCampaigns))]
        public async Task<ActionResult<IEnumerable<CampaignDto>>> GetCampaigns([FromQuery] PaginationResourceParameters parameters, [FromQuery] ActivityStatus status)
        {
            var query = _dbContext.Campaigns.AsNoTracking();

            switch (status)
            {
                case ActivityStatus.Coming:
                    query = query.Where(x => x.StartTime > DateTime.Now);
                    break;
                case ActivityStatus.Ing:
                    query = query
                        .Where(x => x.EndTime > DateTime.Now);
                    break;
                case ActivityStatus.End:
                    query = query.Where(x => x.EndTime <= DateTime.Now);
                    break;
            }
            
            var entities = await query
                .OrderBy(x => x.StartTime)
                .Include(x => x.CampaignFiles)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<CampaignDto>>(entities);
            
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
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(CreateCampaign))]
        public async Task<IActionResult> CreateCampaign([FromBody] CampaignCreateDto dto)
        {
            CampaignCreateDtoValidator validator = new CampaignCreateDtoValidator();
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

                #endregion
                
                var entity = _mapper.Map<Campaign>(dto);
                _dbContext.Campaigns.Add(entity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = entity.CampaignId};
                var returnDto = _mapper.Map<CampaignDto>(entity);
                return CreatedAtAction(nameof(GetCampaign), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{campaignId}", Name = nameof(GetCampaign))]
        public async Task<ActionResult<CampaignDto>> GetCampaign([FromRoute] string campaignId)
        {
            var entity = await _dbContext.Campaigns
                .AsNoTracking()
                .Include(x => x.CampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<CampaignDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{campaignId}", Name = nameof(EditCampaign))]
        public async Task<ActionResult<CampaignDto>> EditCampaign([FromRoute] string campaignId, [FromBody] CampaignEditDto dto)
        {
            CampaignEditDtoValidator validator = new CampaignEditDtoValidator();
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

                #endregion
                
                var entity = await _dbContext.Campaigns
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Campaigns.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {campaignId = updateEntity.CampaignId};
                var returnDto = _mapper.Map<CampaignDto>(updateEntity);
                return CreatedAtAction(nameof(GetCampaign), routeValues, returnDto);
                
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{campaignId}", Name = nameof(DeleteCampaign))]
        public async Task<ActionResult<CampaignDto>> DeleteCampaign([FromRoute] string campaignId)
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
            
            var entity = await _dbContext.Campaigns
                .Include(x => x.CampaignFiles)
                .Include(x => x.GeneralCampaigns)
                .ThenInclude(x => x.GeneralCampaignFiles)
                .Include(x => x.RecruitmentCampaigns)
                .ThenInclude(x => x.RecruitmentCampaignFiles)
                .Include(x => x.RecruitmentCampaigns)
                .ThenInclude(x => x.RecruitmentCampaignOpenings)
                .ThenInclude(x => x.Qualifications)
                .Include(x => x.RecruitmentCampaigns)
                .ThenInclude(x => x.RecruitmentCampaignOpenings)
                .ThenInclude(x => x.Faculties)
                .Include(x => x.RecruitmentCampaigns)
                .ThenInclude(x => x.RecruitmentCampaignOpenings)
                .ThenInclude(x => x.RecruitmentCampaignResumes)
                .ThenInclude(x => x.FileResume)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
            if (entity == null)
            {
                return NotFound();
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    var users = new List<string>();

                    foreach (var act in entity.GeneralCampaigns)
                    {
                        foreach (var attendee in act.GeneralCampaignAttendees)
                        {
                            var user = await _dbContext.Users
                                .AsNoTracking()
                                .Select(x => new {x.Id, x.Email})
                                .SingleOrDefaultAsync(x => x.Id == attendee.ApplicationUserId);
                            if (!users.Contains(user.Email))
                            {
                                users.Add(user.Email);
                            }
                            _dbContext.GeneralCampaignAttendees.Remove(attendee);
                            await _dbContext.SaveChangesAsync();
                        }
                        foreach (var file in act.GeneralCampaignFiles)
                        {
                            _dbContext.GeneralCampaignFiles.Remove(file);
                            await _dbContext.SaveChangesAsync();
                            System.IO.File.Delete(file.Path);
                        }
                    }
                    
                    foreach (var act in entity.RecruitmentCampaigns)
                    {
                        foreach (var opening in act.RecruitmentCampaignOpenings)
                        {
                            foreach (var resume in opening.RecruitmentCampaignResumes)
                            {
                                _dbContext.FileResumes.Remove(resume.FileResume);
                                await _dbContext.SaveChangesAsync();
                                System.IO.File.Delete(resume.FileResume.Path);
                            }
                            _dbContext.Qualifications.RemoveRange(opening.Qualifications);
                            _dbContext.Faculties.RemoveRange(opening.Faculties);
                            _dbContext.RecruitmentCampaignOpenings.Remove(opening);
                            await _dbContext.SaveChangesAsync();
                        }
                        foreach (var file in act.RecruitmentCampaignFiles)
                        {
                            _dbContext.RecruitmentCampaignFiles.Remove(file);
                            await _dbContext.SaveChangesAsync();
                            System.IO.File.Delete(file.Path);
                        }
                    }

                    foreach (var file in entity.CampaignFiles)
                    {
                        _dbContext.CampaignFiles.Remove(file);
                        await _dbContext.SaveChangesAsync();
                        System.IO.File.Delete(file.Path);
                    }

                    _dbContext.Campaigns.Remove(entity);
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
    }
}