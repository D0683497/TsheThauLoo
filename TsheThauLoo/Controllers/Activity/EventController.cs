using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.Event;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Enums;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Activity.Event;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize(Roles = "Administrator")]
    [Route("api/events")]
    public class EventController : ControllerBase
    {
        private readonly ILogger<EventController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public EventController(
            ILogger<EventController> logger, 
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(GetEvents))]
        public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] PaginationResourceParameters parameters, [FromQuery] ActivityStatus status)
        {
            var query = _dbContext.Events.AsNoTracking();

            switch (status)
            {
                case ActivityStatus.Coming:
                    query = query.Where(x => x.RegistrationStartTime != null && x.RegistrationStartTime > DateTime.Now);
                    break;
                case ActivityStatus.Ing:
                    query = query
                        .Where(x => 
                            (x.RegistrationStartTime != null && x.RegistrationStartTime <= DateTime.Now && x.EndTime > DateTime.Now) ||
                            (x.RegistrationStartTime == null && x.EndTime > DateTime.Now));
                    break;
                case ActivityStatus.End:
                    query = query.Where(x => x.EndTime <= DateTime.Now);
                    break;
            }
            
            var entities = await query
                .OrderBy(x => x.StartTime)
                .Include(x => x.EventFiles)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<EventDto>>(entities);
            
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
        [HttpPost(Name = nameof(CreateEvent))]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
        {
            EventCreateDtoValidator validator = new EventCreateDtoValidator();
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
                
                var entity = _mapper.Map<Event>(dto);
                _dbContext.Events.Add(entity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {eventId = entity.EventId};
                var returnDto = _mapper.Map<EventDto>(entity);
                return CreatedAtAction(nameof(GetEvent), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{eventId}", Name = nameof(GetEvent))]
        public async Task<ActionResult<EventDto>> GetEvent([FromRoute] string eventId)
        {
            var entity = await _dbContext.Events
                .AsNoTracking()
                .Include(x => x.EventFiles)
                .SingleOrDefaultAsync(x => x.EventId == eventId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<EventDto>(entity);
            return Ok(dto);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{eventId}", Name = nameof(EditEvent))]
        public async Task<ActionResult<EventDto>> EditEvent([FromRoute] string eventId, [FromBody] EventEditDto dto)
        {
            EventEditDtoValidator validator = new EventEditDtoValidator();
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
                
                var entity = await _dbContext.Events
                    .SingleOrDefaultAsync(x => x.EventId == eventId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Events.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {eventId = updateEntity.EventId};
                var returnDto = _mapper.Map<EventDto>(updateEntity);
                return CreatedAtAction(nameof(GetEvent), routeValues, returnDto);
                
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{eventId}", Name = nameof(DeleteEvent))]
        public async Task<ActionResult<EventDto>> DeleteEvent([FromRoute] string eventId)
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
            
            var entity = await _dbContext.Events
                .Include(x => x.EventFiles)
                .Include(x => x.EventAttendees)
                .SingleOrDefaultAsync(x => x.EventId == eventId);
            if (entity == null)
            {
                return NotFound();
            }

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    foreach (var attendee in entity.EventAttendees)
                    {
                        _dbContext.EventAttendees.Remove(attendee);
                    }
                    
                    // TODO: 寄信通知
                    
                    foreach (var file in entity.EventFiles)
                    {
                        _dbContext.EventFiles.Remove(file);
                        await _dbContext.SaveChangesAsync();
                        System.IO.File.Delete(file.Path);
                    }
                    
                    _dbContext.Events.Remove(entity);
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

        [AuthAuthorize]
        [HttpPost("{eventId}/attendee", Name = nameof(AttendeeEvent))]
        public async Task<IActionResult> AttendeeEvent([FromRoute] string eventId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var user = await _dbContext.Users
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == userId);
            var act = await _dbContext.Events
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.EventId == eventId);

            #region 驗證

            if (act.EnableIdentityConfirmed && !user.IdentityConfirmed)
            {
                return Problem(title: "報名失敗", detail: "需要實名驗證", statusCode: 403);
            }
            if (act.LimitNumberOfPeople != 0)
            {
                var count = await _dbContext.EventAttendees
                    .Where(x => x.Status == AttendeeStatusType.SignUpSuccess)
                    .Where(x => x.EventId == eventId)
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
            if (await _dbContext.EventAttendees.AnyAsync(x => x.EventId == eventId && x.ApplicationUserId == userId))
            {
                return Problem(title: "報名失敗", detail: "已報名", statusCode: 403);
            }

            #endregion

            var entity = new EventAttendee
            {
                Status = act.EnableVerify ? AttendeeStatusType.UnderReview : AttendeeStatusType.SignUpSuccess,
                EventId = eventId,
                ApplicationUserId = userId
            };
            _dbContext.EventAttendees.Add(entity);
            await _dbContext.SaveChangesAsync();
            
            // TODO: 寄信通知

            return NoContent();
        }
        
        private TimeComparisonStatus CompareDate(DateTime firstDate, DateTime secondDate)
        {
            var result = (TimeComparisonStatus) DateTime.Compare(firstDate, secondDate);
            return result;
        }
    }
}
