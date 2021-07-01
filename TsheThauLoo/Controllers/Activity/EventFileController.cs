using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize(Roles = "Administrator")]
    [Route("api/events/{eventId}/files")]
    public class EventFileController : ControllerBase
    {
        private readonly ILogger<EventFileController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public EventFileController(
            ILogger<EventFileController> logger,
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost(Name = nameof(CreateEventFile))]
        public async Task<IActionResult> CreateEventFile([FromRoute] string eventId, [FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
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
               
                var act = await _dbContext.Events
                    .Include(x => x.EventFiles)
                    .SingleOrDefaultAsync(x => x.EventId == eventId);
                if (act == null)
                {
                    return NotFound();
                }

                var entity = _mapper.Map<EventFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        act.EventFiles.Add(entity);
                        _dbContext.Events.Update(act);
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

                return NoContent();

                // var routeValues = new {eventId = act.EventId, fileId = entity.EventFileId};
                // var returnDto = _mapper.Map<FileDto>(entity);
                // return CreatedAtAction(nameof(EventFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}