using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data;
using TsheThauLoo.Models.School;

namespace TsheThauLoo.Controllers
{
    [ApiVersion("1.0")]
    [Route("api")]
    [ApiController]
    public class SchoolController : ControllerBase
    {
        private readonly ILogger<SchoolController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public SchoolController(
            ILogger<SchoolController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("colleges")]
        public async Task<ActionResult<IEnumerable<CollegeDto>>> GetColleges()
        {
            var entities = await _dbContext.Colleges
                .AsNoTrackingWithIdentityResolution()
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<CollegeDto>>(entities);
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("colleges/{collegeId}")]
        public async Task<ActionResult<CollegeDto>> GetCollege([FromRoute] string collegeId)
        {
            var entity = await _dbContext.Colleges
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(x => x.Id == collegeId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<CollegeDto>(entity);
            return Ok(dto);
        }

        [AllowAnonymous]
        [HttpGet("colleges/{collegeId}/departments")]
        public async Task<ActionResult<IEnumerable<DepartmentDto>>> GetDepartments([FromRoute] string collegeId)
        {
            var entity = await _dbContext.Colleges
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Departments)
                .SingleOrDefaultAsync(x => x.Id == collegeId);
            if (entity == null)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<IEnumerable<DepartmentDto>>(entity.Departments);
            return Ok(dtos);
        }

        [AllowAnonymous]
        [HttpGet("departments/{departmentId}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment([FromRoute] string departmentId)
        {
            var entity = await _dbContext.Departments
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(x => x.Id == departmentId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<DepartmentDto>(entity);
            return Ok(dto);
        }

        [AllowAnonymous]
        [HttpGet("colleges/{collegeId}/departments/{departmentId}")]
        public async Task<ActionResult<DepartmentDto>> GetDepartment([FromRoute] string collegeId, [FromRoute] string departmentId)
        {
            var entity = await _dbContext.Departments
                .AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(x => x.CollegeId == collegeId && x.Id == departmentId);
            if (entity == null)
            {
                return NotFound();
            }
            var dto = _mapper.Map<DepartmentDto>(entity);
            return Ok(dto);
        }
    }
}
