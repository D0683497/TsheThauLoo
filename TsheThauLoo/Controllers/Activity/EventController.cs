using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.Event;
using TsheThauLoo.Entities.Activity;
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

        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost(Name = nameof(CreateEvent))]
        public async Task<IActionResult> CreateEvent([FromBody] EventCreateDto dto)
        {
            EventCreateDtoValidator validator = new EventCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var entity = _mapper.Map<Event>(dto);
                _dbContext.Events.Add(entity);
                await _dbContext.SaveChangesAsync();
                return NoContent();
            }
            return BadRequest(result.Errors);
        }
    }
}