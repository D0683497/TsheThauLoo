using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.MyCampaign;
using TsheThauLoo.Dtos.Activity.MyEvent;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;

namespace TsheThauLoo.Controllers
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/my")]
    public class MyController : ControllerBase
    {
        private readonly ILogger<MyController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public MyController(
            ILogger<MyController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AuthAuthorize(Roles = "Manager")]
        [HttpGet("company", Name = nameof(MyCompany))]
        public async Task<ActionResult<CompanyDto>> MyCompany()
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var manager = await _dbContext.Managers
                .AsNoTracking()
                .Include(x => x.Company)
                .Include(x => x.Company.CompanyLogo)
                .Include(x => x.Company.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
            if (manager.Company == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<CompanyDto>(manager.Company);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpGet("events", Name = nameof(MyEvents))]
        public async Task<ActionResult<IEnumerable<MyEventDto>>> MyEvents([FromQuery] PaginationResourceParameters parameters)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var query = _dbContext.Events
                .AsNoTracking()
                .Include(x => x.EventAttendees.Where(attendee => attendee.ApplicationUserId == userId))
                .Where(x => x.EventAttendees.Any(attendee => attendee.ApplicationUserId == userId));
            var entities = await query
                .OrderBy(x => x.StartTime)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<MyEventDto>>(entities);
            
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
        
        [AuthAuthorize]
        [HttpGet("events/{eventId}", Name = nameof(MyEvent))]
        public async Task<ActionResult<MyEventDto>> MyEvent([FromRoute] string eventId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Events
                .AsNoTracking()
                .Include(x => x.EventAttendees.Where(attendee => attendee.ApplicationUserId == userId))
                .Where(x => x.EventAttendees.Any(attendee => attendee.ApplicationUserId == userId))
                .SingleOrDefaultAsync(x => x.EventId == eventId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<MyEventDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpGet("campaigns", Name = nameof(MyCampaigns))]
        public async Task<ActionResult<IEnumerable<MyCampaignDto>>> MyCampaigns([FromQuery] PaginationResourceParameters parameters)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var query = _dbContext.Campaigns
                .AsNoTracking()
                .Include(x => x.GeneralCampaigns
                    .Where(y => y.GeneralCampaignAttendees.Any(attendee => attendee.ApplicationUserId == userId)))
                .ThenInclude(x => x.GeneralCampaignAttendees.Where(attendee => attendee.ApplicationUserId == userId));
            var entities = await query
                .OrderBy(x => x.StartTime)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();

            var dtos = _mapper.Map<IEnumerable<MyCampaignDto>>(entities);
            
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
        
        [AuthAuthorize]
        [HttpGet("campaigns/{campaignId}", Name = nameof(MyCampaign))]
        public async Task<ActionResult<MyCampaignDto>> MyCampaign([FromRoute] string campaignId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.Campaigns
                .AsNoTracking()
                .Include(x => x.GeneralCampaigns
                    .Where(y => y.GeneralCampaignAttendees.Any(attendee => attendee.ApplicationUserId == userId)))
                .ThenInclude(x => x.GeneralCampaignAttendees.Where(attendee => attendee.ApplicationUserId == userId))
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<MyCampaignDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize]
        [HttpGet("campaigns/{campaignId}/general/{generalId}", Name = nameof(MyGeneral))]
        public async Task<ActionResult<MyGeneralCampaignDto>> MyGeneral([FromRoute] string campaignId, [FromRoute] string generalId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var entity = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .Include(x => x.GeneralCampaignAttendees.Where(attendee => attendee.ApplicationUserId == userId))
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<MyGeneralCampaignDto>(entity);
            return Ok(dto);
        }
    }
}