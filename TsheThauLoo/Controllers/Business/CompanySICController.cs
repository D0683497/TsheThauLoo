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
    [Route("api/companies/{companyId}/sic")]
    public class CompanySICController : ControllerBase
    {
        private readonly ILogger<CompanySICController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public CompanySICController(
            ILogger<CompanySICController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
         [AuthAuthorize(Roles = "Manager")]
        [HttpPost(Name = nameof(CreateIndustrialClassification))]
        public async Task<IActionResult> CreateIndustrialClassification([FromRoute] string companyId, [FromBody] IndustrialClassificationCreateDto dto)
        {
            IndustrialClassificationCreateDtoValidator validator = new IndustrialClassificationCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var company = await _dbContext.Companies
                    .Include(x => x.CompanyLogo)
                    .Include(x => x.IndustrialClassifications)
                    .Include(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CompanyId == companyId);
                if (company == null)
                {
                    return NotFound();
                }
                var manager = company.Managers
                    .SingleOrDefault(x => x.ApplicationUserId == userId);
                if (manager == null)
                {
                    return Problem(title: "禁止修改", detail: "非該公司管理者", statusCode: 403);
                }
                if (!manager.ManagerConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "企業使用者尚未驗證", statusCode: 403);
                }
                var entity = _mapper.Map<IndustrialClassification>(dto);
                company.IndustrialClassifications.Add(entity);
                _dbContext.Companies.Update(company);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {companyId = company.CompanyId};
                var returnDto = _mapper.Map<CompanyDto>(company);
                return CreatedAtRoute(nameof(CompanyController.GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
         [AuthAuthorize(Roles = "Manager")]
        [HttpPost("{industrialClassificationId}", Name = nameof(EditIndustrialClassification))]
        public async Task<IActionResult> EditIndustrialClassification([FromRoute] string companyId, [FromRoute] string industrialClassificationId, [FromBody] IndustrialClassificationEditDto dto)
        {
            IndustrialClassificationEditDtoValidator validator = new IndustrialClassificationEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var company = await _dbContext.Companies
                    .Include(x => x.CompanyLogo)
                    .Include(x => x.IndustrialClassifications)
                    .Include(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CompanyId == companyId);
                if (company == null)
                {
                    return NotFound();
                }
                var manager = company.Managers
                    .SingleOrDefault(x => x.ApplicationUserId == userId);
                if (manager == null)
                {
                    return Problem(title: "禁止修改", detail: "非該公司管理者", statusCode: 403);
                }
                if (!manager.ManagerConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "企業使用者尚未驗證", statusCode: 403);
                }
                var entity = company.IndustrialClassifications
                    .SingleOrDefault(x => x.IndustrialClassificationId == industrialClassificationId);
                if (entity == null)
                {
                    return NotFound();
                }
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.IndustrialClassifications.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {companyId = company.CompanyId};
                var returnDto = _mapper.Map<CompanyDto>(company);
                return CreatedAtRoute(nameof(CompanyController.GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpDelete("{industrialClassificationId}", Name = nameof(DeleteIndustrialClassification))]
        public async Task<IActionResult> DeleteIndustrialClassification([FromRoute] string companyId, [FromRoute] string industrialClassificationId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var company = await _dbContext.Companies
                .Include(x => x.IndustrialClassifications)
                .Include(x => x.Managers)
                .SingleOrDefaultAsync(x => x.CompanyId == companyId);
            if (company == null)
            {
                return NotFound();
            }
            var manager = company.Managers
                .SingleOrDefault(x => x.ApplicationUserId == userId);
            if (manager == null)
            {
                return Problem(title: "禁止修改", detail: "非該公司管理者", statusCode: 403);
            }
            if (!manager.ManagerConfirmed)
            {
                return Problem(title: "禁止修改", detail: "企業使用者尚未驗證", statusCode: 403);
            }
            var entity = company.IndustrialClassifications
                .SingleOrDefault(x => x.IndustrialClassificationId == industrialClassificationId);
            if (entity == null)
            {
                return NotFound();
            }
            _dbContext.IndustrialClassifications.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
    }
}