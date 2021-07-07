using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Business
{
    [ApiController]
    [AllowAnonymous]
    [Route("api/companies/{companyId}/logo")]
    public class CompanyLogoController : ControllerBase
    {
        private readonly ILogger<CompanyLogoController> _logger;
        private readonly TsheThauLooDbContext _dbContext;
        private readonly IMapper _mapper;

        public CompanyLogoController(
            ILogger<CompanyLogoController> logger, 
            TsheThauLooDbContext dbContext, 
            IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(CompanyLogo))]
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
        [HttpPost(Name = nameof(CreateCompanyLogo))]
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
        [HttpDelete(Name = nameof(DeleteCompanyLogo))]
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
    }
}