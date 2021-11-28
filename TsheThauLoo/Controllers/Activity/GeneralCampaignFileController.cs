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
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.File;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/campaigns/{campaignId}/general/{generalId}/files")]
    public class GeneralCampaignFileController : ControllerBase
    {
        private readonly ILogger<GeneralCampaignFileController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public GeneralCampaignFileController(
            ILogger<GeneralCampaignFileController> logger, 
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost(Name = nameof(CreateGeneralFile))]
        public async Task<IActionResult> CreateGeneralFile([FromRoute] string campaignId, [FromRoute] string generalId, [FromForm] FileCreateDto dto)
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
               
                var act = await _dbContext.GeneralCampaigns
                    .Include(x => x.GeneralCampaignFiles)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
                if (act == null)
                {
                    return NotFound();
                }

                var entity = _mapper.Map<GeneralCampaignFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        act.GeneralCampaignFiles.Add(entity);
                        _dbContext.GeneralCampaigns.Update(act);
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
                
                var routeValues = new {campaignId = act.CampaignId, generalId = act.GeneralCampaignId, fileId = entity.GeneralCampaignFileId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(GeneralFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{fileId}", Name = nameof(GeneralFile))]
        public async Task<IActionResult> GeneralFile([FromRoute] string campaignId, [FromRoute] string generalId, [FromRoute] string fileId)
        {
            var act = await _dbContext.GeneralCampaigns
                .AsNoTracking()
                .Include(x => x.GeneralCampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (act == null)
            {
                return NotFound();
            }

            var entity = act.GeneralCampaignFiles
                .SingleOrDefault(x => x.GeneralCampaignFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{fileId}", Name = nameof(EditGeneralFile))]
        public async Task<IActionResult> EditGeneralFile([FromRoute] string campaignId, [FromRoute] string generalId, [FromRoute] string fileId, [FromBody] FileEditDto dto)
        {
            FileEditDtoValidator validator = new FileEditDtoValidator();
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
                
                var act = await _dbContext.GeneralCampaigns
                    .Include(x => x.GeneralCampaignFiles)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
                if (act == null)
                {
                    return NotFound();
                }
                
                var entity = act.GeneralCampaignFiles
                    .SingleOrDefault(x => x.GeneralCampaignFileId == fileId);
                if (entity == null)
                {
                    return NotFound();
                }
                
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.GeneralCampaignFiles.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                
                var routeValues = new {campaignId = act.CampaignId, generalId = act.GeneralCampaignId, fileId = entity.GeneralCampaignFileId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(GeneralFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{fileId}", Name = nameof(DeleteGeneralFile))]
        public async Task<IActionResult> DeleteGeneralFile([FromRoute] string campaignId, [FromRoute] string generalId, [FromRoute] string fileId)
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
            
            var act = await _dbContext.GeneralCampaigns
                .Include(x => x.GeneralCampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.GeneralCampaignId == generalId);
            if (act == null)
            {
                return NotFound();
            }
                
            var entity = act.GeneralCampaignFiles
                .SingleOrDefault(x => x.GeneralCampaignFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.GeneralCampaignFiles.Remove(entity);
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