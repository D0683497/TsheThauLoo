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
    [Route("api/campaigns/{campaignId}/recruitment/{recruitmentId}/files")]
    public class RecruitmentCampaignFileController : ControllerBase
    {
        private readonly ILogger<RecruitmentCampaignFileController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public RecruitmentCampaignFileController(
            ILogger<RecruitmentCampaignFileController> logger, 
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [RequestFormLimits(ValueLengthLimit  = int.MaxValue, MultipartBodyLengthLimit = int.MaxValue)]
        [HttpPost(Name = nameof(CreateRecruitmentFile))]
        public async Task<IActionResult> CreateRecruitmentFile([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromForm] FileCreateDto dto)
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
               
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignFiles)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                if (act == null)
                {
                    return NotFound();
                }

                var entity = _mapper.Map<RecruitmentCampaignFile>(dto);

                #region 處理檔案

                await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        using (var stream = new FileStream(entity.Path, FileMode.Create))
                        {
                            await dto.FileData.CopyToAsync(stream);
                        }
                        act.RecruitmentCampaignFiles.Add(entity);
                        _dbContext.RecruitmentCampaigns.Update(act);
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
                
                var routeValues = new {campaignId = act.CampaignId, recruitmentId = act.RecruitmentCampaignId, fileId = entity.RecruitmentCampaignFileId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(RecruitmentFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AllowAnonymous]
        [HttpGet("{fileId}", Name = nameof(RecruitmentFile))]
        public async Task<IActionResult> RecruitmentFile([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string fileId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            if (act == null)
            {
                return NotFound();
            }

            var entity = act.RecruitmentCampaignFiles
                .SingleOrDefault(x => x.RecruitmentCampaignFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }
            
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(entity.Path), entity.Type, $"{entity.Name}{entity.Extension}");
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpPost("{fileId}", Name = nameof(EditRecruitmentFile))]
        public async Task<IActionResult> EditRecruitmentFile([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string fileId, [FromBody] FileEditDto dto)
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
                
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignFiles)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                if (act == null)
                {
                    return NotFound();
                }
                
                var entity = act.RecruitmentCampaignFiles
                    .SingleOrDefault(x => x.RecruitmentCampaignFileId == fileId);
                if (entity == null)
                {
                    return NotFound();
                }
                
                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.RecruitmentCampaignFiles.Update(updateEntity);
                await _dbContext.SaveChangesAsync();
                
                var routeValues = new {campaignId = act.CampaignId, recruitmentId = act.RecruitmentCampaignId, fileId = entity.RecruitmentCampaignFileId};
                var returnDto = _mapper.Map<FileDto>(entity);
                return CreatedAtAction(nameof(RecruitmentFile), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator")]
        [HttpDelete("{fileId}", Name = nameof(DeleteRecruitmentFile))]
        public async Task<IActionResult> DeleteRecruitmentFile([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string fileId)
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
            
            var act = await _dbContext.RecruitmentCampaigns
                .Include(x => x.RecruitmentCampaignFiles)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            if (act == null)
            {
                return NotFound();
            }
                
            var entity = act.RecruitmentCampaignFiles
                .SingleOrDefault(x => x.RecruitmentCampaignFileId == fileId);
            if (entity == null)
            {
                return NotFound();
            }

            #region 處理檔案

            await using (var transaction = await _dbContext.Database.BeginTransactionAsync())
            {
                try
                {
                    _dbContext.RecruitmentCampaignFiles.Remove(entity);
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