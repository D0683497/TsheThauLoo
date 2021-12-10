using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data;
using TsheThauLoo.Entities.School;
using TsheThauLoo.Models.School;

namespace TsheThauLoo.Controllers;

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

    [HttpPost("colleges")]
    public async Task<IActionResult> CreateCollege([FromBody] CreateCollegeDto dto)
    {
        var entity = _mapper.Map<CreateCollegeDto, College>(dto);
        _dbContext.Colleges.Add(entity);
        await _dbContext.SaveChangesAsync();
        var returnDto = _mapper.Map<CollegeDto>(entity);
        var routeValues = new { collegeId = returnDto.Id };
        return CreatedAtAction(nameof(GetCollege), routeValues, returnDto);
    }

    [HttpPost("colleges/{collegeId}/departments")]
    public async Task<IActionResult> CreateDepartment([FromRoute] string collegeId, [FromBody] CreateDepartmentDto dto)
    {
        if (!await _dbContext.Colleges.AnyAsync(x => x.Id == collegeId))
        {
            return NotFound();
        }
        var entity = _mapper.Map<CreateDepartmentDto, Department>(dto);
        entity.CollegeId = collegeId;
        _dbContext.Departments.Add(entity);
        await _dbContext.SaveChangesAsync();
        var returnDto = _mapper.Map<DepartmentDto>(entity);
        var routeValues = new { collegeId, departmentId = returnDto.Id };
        return CreatedAtAction(nameof(GetDepartment), routeValues, returnDto);
    }
        
    [HttpPut("colleges/{collegeId}")]
    public async Task<IActionResult> UpdateCollege([FromRoute] string collegeId, [FromBody] UpdateCollegeDto dto)
    {
        var entity = await _dbContext.Colleges
            .SingleOrDefaultAsync(x => x.Id == collegeId);
        if (entity == null)
        {
            return NotFound();
        }
        var updateEntity = _mapper.Map(dto, entity);
        _dbContext.Colleges.Update(updateEntity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
    
    [HttpPut("departments/{departmentId}")]
    public async Task<IActionResult> UpdateDepartment([FromRoute] string departmentId, [FromBody] UpdateDepartmentDto dto)
    {
        var entity = await _dbContext.Departments
            .SingleOrDefaultAsync(x => x.Id == departmentId);
        if (entity == null)
        {
            return NotFound();
        }
        var updateEntity = _mapper.Map(dto, entity);
        _dbContext.Departments.Update(updateEntity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("colleges/{collegeId}/departments/{departmentId}")]
    public async Task<IActionResult> UpdateDepartment([FromRoute] string collegeId, [FromRoute] string departmentId, [FromBody] UpdateDepartmentDto dto)
    {
        var entity = await _dbContext.Departments
            .SingleOrDefaultAsync(x => x.CollegeId == collegeId && x.Id == departmentId);
        if (entity == null)
        {
            return NotFound();
        }
        var updateEntity = _mapper.Map(dto, entity);
        _dbContext.Departments.Update(updateEntity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("colleges/{collegeId}")]
    public async Task<IActionResult> DeleteCollege([FromRoute] string collegeId)
    {
        var entity = await _dbContext.Colleges
            .SingleOrDefaultAsync(x => x.Id == collegeId);
        if (entity == null)
        {
            return NotFound();
        }
        _dbContext.Colleges.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("departments/{departmentId}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] string departmentId)
    {
        var entity = await _dbContext.Departments
            .SingleOrDefaultAsync(x => x.Id == departmentId);
        if (entity == null)
        {
            return NotFound();
        }
        _dbContext.Departments.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("colleges/{collegeId}/departments/{departmentId}")]
    public async Task<IActionResult> DeleteDepartment([FromRoute] string collegeId, [FromRoute] string departmentId)
    {
        var entity = await _dbContext.Departments
            .SingleOrDefaultAsync(x => x.CollegeId == collegeId && x.Id == departmentId);
        if (entity == null)
        {
            return NotFound();
        }
        _dbContext.Departments.Remove(entity);
        await _dbContext.SaveChangesAsync();
        return NoContent();
    }
}