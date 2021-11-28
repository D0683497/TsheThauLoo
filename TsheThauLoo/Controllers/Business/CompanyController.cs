using System.Collections.Generic;
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
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Company;

namespace TsheThauLoo.Controllers.Business
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/companies")]
    public class CompanyController : ControllerBase
    {
        private readonly ILogger<CompanyController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public CompanyController(
            ILogger<CompanyController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet(Name = nameof(CompanyList))]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CompanyList([FromQuery] PaginationResourceParameters parameters)
        {
            var entities = await _dbContext.Companies
                .AsNoTracking()
                .Include(x => x.CompanyLogo)
                .Include(x => x.IndustrialClassifications)
                .OrderBy(x => x.RegistrationNumber)
                .Skip(parameters.PageIndex * parameters.PageSize)
                .Take(parameters.PageSize)
                .ToListAsync();
            var dtos = _mapper.Map<IEnumerable<CompanyDto>>(entities);
            
            #region 分頁資訊

            var length = await _dbContext.Companies.CountAsync();
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
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpPost("{companyId}", Name = nameof(CompanyEdit))]
        public async Task<ActionResult<CompanyDto>> CompanyEdit([FromRoute] string companyId, [FromBody] CompanyEditDto dto)
        {
            CompanyEditDtoValidator validator = new CompanyEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var company = await _dbContext.Companies
                    .Include(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CompanyId == companyId);
                var manager = company.Managers
                    .SingleOrDefault(x => x.ApplicationUserId == userId);

                #region 驗證

                if (manager == null)
                {
                    return Problem(title: "禁止修改", detail: "非該公司管理者", statusCode: 403);
                }
                if (!manager.ManagerConfirmed)
                {
                    return Problem(title: "禁止修改", detail: "企業使用者尚未驗證", statusCode: 403);
                }
                if (company.CompanyConfirmed)
                {
                    if (company.Name != dto.Name)
                    {
                        result.Errors.Add(new ValidationFailure("name", "名稱禁止修改"));
                    }
                    if (company.RegistrationNumber != dto.RegistrationNumber)
                    {
                        result.Errors.Add(new ValidationFailure("registrationNumber", "統一編號禁止修改"));
                    }
                }
                else
                {
                    if (company.RegistrationNumber != dto.RegistrationNumber)
                    {
                        if (await _dbContext.Companies.AnyAsync(x => x.RegistrationNumber == dto.RegistrationNumber))
                        {
                            result.Errors.Add(new ValidationFailure("registrationNumber", "統一編號已經被使用"));
                        }
                    }
                }
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                #endregion
                
                var updateEntity = _mapper.Map(dto, company);
                _dbContext.Companies.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                var routeValues = new {companyId = updateEntity.CompanyId};
                var returnDto = _mapper.Map<CompanyDto>(updateEntity);
                return CreatedAtAction(nameof(GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
    }
}