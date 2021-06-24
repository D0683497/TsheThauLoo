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
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Parameters;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Company;
using TsheThauLoo.Validator.File;

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
        
        [AllowAnonymous]
        [HttpGet("{companyId}/logo", Name = nameof(CompanyLogo))]
        public async Task<IActionResult> CompanyLogo([FromRoute] string companyId)
        {
            var entity = await _dbContext.CompanyLogos
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.CompanyId == companyId);
            if (entity == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost("{companyId}/logo", Name = nameof(CreateCompanyLogo))]
        public async Task<IActionResult> CreateCompanyLogo([FromRoute] string companyId, [FromForm] FileCreateDto dto)
        {
            FileCreateDtoValidator validator = new FileCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var company = await _dbContext.Companies
                    .Include(x => x.CompanyLogo)
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

                var entity = _mapper.Map(dto, company.CompanyLogo);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        company.CompanyLogo = entity;
                        _dbContext.Companies.Update(company);
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

                var routeValues = new {companyId = entity.CompanyId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(CompanyLogo), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpDelete("{companyId}/logo", Name = nameof(DeleteCompanyLogo))]
        public async Task<IActionResult> DeleteCompanyLogo([FromRoute] string companyId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var company = await _dbContext.Companies
                .Include(x => x.CompanyLogo)
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

            var entity = company.CompanyLogo;

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.CompanyLogos.Remove(entity);
                    await _dbContext.SaveChangesAsync();
                    System.IO.File.Delete(entity.Path);
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
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpPost("{companyId}/sic", Name = nameof(CreateIndustrialClassification))]
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
                return CreatedAtAction(nameof(GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpPost("{companyId}/sic/{industrialClassificationId}", Name = nameof(EditIndustrialClassification))]
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
                return CreatedAtAction(nameof(GetCompany), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Manager")]
        [HttpDelete("{companyId}/sic/{industrialClassificationId}", Name = nameof(DeleteIndustrialClassification))]
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