using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Company;

namespace TsheThauLoo.Controllers.Business
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ILogger<CompaniesController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public CompaniesController(
            ILogger<CompaniesController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AuthAuthorize(Roles = "Manager")]
        [HttpPost(Name = nameof(CompanyCreate))]
        public async Task<ActionResult<CompanyDto>> CompanyCreate([FromBody] CompanyCreateDto dto)
        {
            CompanyCreateDtoValidator validator = new CompanyCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var manager = await _dbContext.Managers
                    .Include(x => x.Company)
                    .SingleOrDefaultAsync(x => x.ApplicationUserId == userId);
                
                #region 驗證
                
                if (!manager.ManagerConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "企業使用者尚未驗證", statusCode: 403);
                }
                if (manager.Company != null)
                {
                    return Problem(title: "禁止修改", detail: "無法新增多個公司", statusCode: 403);
                }
                if (await _dbContext.Companies.AnyAsync(x => x.RegistrationNumber == dto.RegistrationNumber))
                {
                    result.Errors.Add(new ValidationFailure("registrationNumber", "統一編號已經被使用"));
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var entity = _mapper.Map<Company>(dto);
                manager.Company = entity;
                _dbContext.Managers.Update(manager);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {companyId = entity.CompanyId};
                var returnDto = _mapper.Map<CompanyDto>(entity);
                return CreatedAtAction(nameof(GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        public async Task<ActionResult<CompanyDto>> GetCompany([FromRoute] string companyId)
        {
            var entity = await _dbContext.Companies
                .AsNoTracking()
                .Include(x => x.CompanyLogo)
                .Include(x => x.IndustrialClassifications)
                .SingleOrDefaultAsync(x => x.CompanyId == companyId);
            var dto = _mapper.Map<CompanyDto>(entity);
            return Ok(dto);
        }
    }
}