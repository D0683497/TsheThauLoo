using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TsheThauLoo.Data;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Job;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Enums;
using TsheThauLoo.Utilities;
using TsheThauLoo.Validator.Activity.RecruitmentCampaign;

namespace TsheThauLoo.Controllers.Activity
{
    [ApiController]
    [AuthAuthorize]
    [Route("api/campaigns/{campaignId}/recruitment/{recruitmentId}/openings")]
    public class RecruitmentCampaignOpeningController : ControllerBase
    {
        private readonly ILogger<RecruitmentCampaignOpeningController> _logger;
        private readonly IMapper _mapper;
        private readonly TsheThauLooDbContext _dbContext;

        public RecruitmentCampaignOpeningController(
            ILogger<RecruitmentCampaignOpeningController> logger, 
            IMapper mapper, 
            TsheThauLooDbContext dbContext)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
        }
        
        [AllowAnonymous]
        [HttpGet(Name = nameof(Openings))]
        public async Task<ActionResult<IEnumerable<RecruitmentCampaignOpeningDto>>> Openings([FromRoute] string campaignId, [FromRoute] string recruitmentId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings)
                .ThenInclude(x => x.Qualifications)
                .Include(x => x.RecruitmentCampaignOpenings)
                .ThenInclude(x => x.Faculties)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var dtos = _mapper.Map<IEnumerable<RecruitmentCampaignOpeningDto>>(act.RecruitmentCampaignOpenings);
            return Ok(dtos);
        }
        
        [AllowAnonymous]
        [HttpGet("{openingId}", Name = nameof(Opening))]
        public async Task<ActionResult<RecruitmentCampaignOpeningDto>> Opening([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Qualifications)
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Faculties)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings.First();
            if (entity == null)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<RecruitmentCampaignOpeningDto>(entity);
            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost(Name = nameof(CreateOpening))]
        public async Task<IActionResult> CreateOpening([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromBody] RecruitmentCampaignOpeningCreateDto dto)
        {
            RecruitmentCampaignOpeningCreateDtoValidator validator = new RecruitmentCampaignOpeningCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var entity = _mapper.Map<RecruitmentCampaignOpening>(dto);
                act.RecruitmentCampaignOpenings.Add(entity);
                _dbContext.RecruitmentCampaigns.Update(act);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId = act.CampaignId, recruitmentId = act.RecruitmentCampaignId, openingId = entity.RecruitmentCampaignOpeningId};
                var returnDto = _mapper.Map<RecruitmentCampaignOpeningDto>(entity);
                return CreatedAtAction(nameof(Opening), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost("{openingId}", Name = nameof(EditOpening))]
        public async Task<IActionResult> EditOpening([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromBody] RecruitmentCampaignOpeningEditDto dto)
        {
            RecruitmentCampaignOpeningEditDtoValidator validator = new RecruitmentCampaignOpeningEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Qualifications)
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Faculties)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                var entity = act.RecruitmentCampaignOpenings.First();
                if (entity == null)
                {
                    return NotFound();
                }

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.RecruitmentCampaignOpenings.Update(updateEntity);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId = act.CampaignId, recruitmentId = act.RecruitmentCampaignId, openingId = entity.RecruitmentCampaignOpeningId};
                var returnDto = _mapper.Map<RecruitmentCampaignOpeningDto>(entity);
                return CreatedAtAction(nameof(Opening), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }

        [AllowAnonymous]
        [HttpGet("{openingId}/faculties", Name = nameof(Faculties))]
        public async Task<ActionResult<IEnumerable<FacultyDto>>> Faculties([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Faculties)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Faculties;
            var dtos = _mapper.Map<IEnumerable<FacultyDto>>(entity);
            return Ok(dtos);
        }
        
        [AllowAnonymous]
        [HttpGet("{openingId}/faculties/{facultyId}", Name = nameof(Faculty))]
        public async Task<ActionResult<FacultyDto>> Faculty([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string facultyId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Faculties)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Faculties
                .SingleOrDefault(x => x.FacultyId == facultyId);
            if (entity == null)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<FacultyDto>(entity);
            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost("{openingId}/faculties", Name = nameof(CreateFaculty))]
        public async Task<IActionResult> CreateFaculty([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromBody] FacultyCreateDto dto)
        {
            FacultyCreateDtoValidator validator = new FacultyCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Faculties)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                var opening = act.RecruitmentCampaignOpenings.First();

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var entity = _mapper.Map<Faculty>(dto);
                opening.Faculties.Add(entity);
                _dbContext.RecruitmentCampaigns.Update(act);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId, recruitmentId, openingId, facultyId = entity.FacultyId};
                var returnDto = _mapper.Map<FacultyDto>(entity);
                return CreatedAtAction(nameof(Faculty), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost("{openingId}/faculties/{facultyId}", Name = nameof(EditFaculty))]
        public async Task<IActionResult> EditFaculty([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string facultyId, [FromBody] FacultyEditDto dto)
        {
            FacultyEditDtoValidator validator = new FacultyEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Faculties)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                var entity = act.RecruitmentCampaignOpenings
                    .First()
                    .Faculties
                    .SingleOrDefault(x => x.FacultyId == facultyId);
                if (entity == null)
                {
                    return NotFound();
                }

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Faculties.Update(updateEntity);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId, recruitmentId, openingId, facultyId};
                var returnDto = _mapper.Map<FacultyDto>(entity);
                return CreatedAtAction(nameof(Faculty), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpDelete("{openingId}/faculties/{facultyId}", Name = nameof(DeleteFaculty))]
        public async Task<IActionResult> DeleteFaculty([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string facultyId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var role = User.Claims
                .Single(p => p.Type == ClaimTypes.Role).Value;
            var act = await _dbContext.RecruitmentCampaigns
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Faculties)
                .Include(x => x.Company)
                .ThenInclude(x => x.Managers)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Faculties
                .SingleOrDefault(x => x.FacultyId == facultyId);
            if (entity == null)
            {
                return NotFound();
            }
            
            #region 驗證

            if (role == RoleType.Manager.ToString("G"))
            {
                if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                {
                    return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                }
            }

            #endregion

            _dbContext.Faculties.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }

        [AllowAnonymous]
        [HttpGet("{openingId}/qualifications", Name = nameof(Qualifications))]
        public async Task<ActionResult<IEnumerable<QualificationDto>>> Qualifications([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Qualifications)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Qualifications;
            var dtos = _mapper.Map<IEnumerable<QualificationDto>>(entity);
            return Ok(dtos);
        }
        
        [AllowAnonymous]
        [HttpGet("{openingId}/qualifications/{qualificationId}", Name = nameof(Qualification))]
        public async Task<ActionResult<QualificationDto>> Qualification([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string qualificationId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Qualifications)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Qualifications
                .SingleOrDefault(x => x.QualificationId == qualificationId);
            if (entity == null)
            {
                return NotFound();
            }
            var dtos = _mapper.Map<QualificationDto>(entity);
            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost("{openingId}/qualifications", Name = nameof(CreateQualification))]
        public async Task<IActionResult> CreateQualification([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromBody] QualificationCreateDto dto)
        {
            QualificationCreateDtoValidator validator = new QualificationCreateDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Qualifications)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                var opening = act.RecruitmentCampaignOpenings.First();

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var entity = _mapper.Map<Qualification>(dto);
                opening.Qualifications.Add(entity);
                _dbContext.RecruitmentCampaigns.Update(act);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId, recruitmentId, openingId, qualificationId = entity.QualificationId};
                var returnDto = _mapper.Map<QualificationDto>(entity);
                return CreatedAtAction(nameof(Qualification), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpPost("{openingId}/qualifications/{qualificationId}", Name = nameof(EditQualification))]
        public async Task<IActionResult> EditQualification([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string qualificationId, [FromBody] QualificationEditDto dto)
        {
            QualificationEditDtoValidator validator = new QualificationEditDtoValidator();
            ValidationResult result = await validator.ValidateAsync(dto);
            if (result.IsValid)
            {
                var userId = User.Claims
                    .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
                var role = User.Claims
                    .Single(p => p.Type == ClaimTypes.Role).Value;
                var act = await _dbContext.RecruitmentCampaigns
                    .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                    .ThenInclude(x => x.Qualifications)
                    .Include(x => x.Company)
                    .ThenInclude(x => x.Managers)
                    .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
                var entity = act.RecruitmentCampaignOpenings
                    .First()
                    .Qualifications
                    .SingleOrDefault(x => x.QualificationId == qualificationId);
                if (entity == null)
                {
                    return NotFound();
                }

                #region 驗證

                if (role == RoleType.Manager.ToString("G"))
                {
                    if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                    {
                        return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                    }
                }

                #endregion

                var updateEntity = _mapper.Map(dto, entity);
                _dbContext.Qualifications.Update(updateEntity);
                await _dbContext.SaveChangesAsync();

                var routeValues = new {campaignId, recruitmentId, openingId, qualificationId};
                var returnDto = _mapper.Map<QualificationDto>(entity);
                return CreatedAtAction(nameof(Qualification), routeValues, returnDto);
            }
            return BadRequest(result.Errors);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpDelete("{openingId}/qualifications/{qualificationId}", Name = nameof(DeleteQualification))]
        public async Task<IActionResult> DeleteQualification([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromRoute] string qualificationId)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var role = User.Claims
                .Single(p => p.Type == ClaimTypes.Role).Value;

            var act = await _dbContext.RecruitmentCampaigns
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.Qualifications)
                .Include(x => x.Company)
                .ThenInclude(x => x.Managers)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var entity = act.RecruitmentCampaignOpenings
                .First()
                .Qualifications
                .SingleOrDefault(x => x.QualificationId == qualificationId);
            if (entity == null)
            {
                return NotFound();
            }
            
            #region 驗證

            if (role == RoleType.Manager.ToString("G"))
            {
                if (!act.Company.Managers.Any(x => x.ApplicationUserId == userId))
                {
                    return Problem(title: "禁止新增", detail: "非此活動邀請企業", statusCode: 403);
                }
            }

            #endregion

            _dbContext.Qualifications.Remove(entity);
            await _dbContext.SaveChangesAsync();
            return NoContent();
        }
        
        [AuthAuthorize]
        [HttpPost("{openingId}/delivery", Name = nameof(DeliveryResume))]
        public async Task<IActionResult> DeliveryResume([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId, [FromBody] ResumeDeliveryDto dto)
        {
            var userId = User.Claims
                .Single(p => p.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").Value;
            var resume = await _dbContext.FileResumes
                .SingleOrDefaultAsync(x => x.ApplicationUserId == userId && x.FileResumeId == dto.ResumeId);
            
            var act = await _dbContext.RecruitmentCampaigns
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.RecruitmentCampaignResumes)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var opening = act.RecruitmentCampaignOpenings.First();

            resume.IsArchive = true;
            opening.RecruitmentCampaignResumes.Add(new RecruitmentCampaignResume
            {
                Type = act.EnableReview ? ResumeReviewType.Pending : ResumeReviewType.Solved,
                FileResumeId = dto.ResumeId,
                FileResume = resume
            });

            _dbContext.FileResumes.Update(resume);
            _dbContext.RecruitmentCampaignOpenings.Update(opening);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpGet("{openingId}/delivery", Name = nameof(DeliveryResumeList))]
        public async Task<IActionResult> DeliveryResumeList([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.RecruitmentCampaignResumes)
                .ThenInclude(x => x.FileResume)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var opening = act.RecruitmentCampaignOpenings.First();
            
            var dtos = _mapper.Map<IEnumerable<ResumeDeliveryListDto>>(opening.RecruitmentCampaignResumes);

            return Ok(dtos);
        }
        
        [AuthAuthorize(Roles = "Administrator,Manager")]
        [HttpGet("{openingId}/delivery/{resumeId}", Name = nameof(DeliveryResumeDownload))]
        public async Task<IActionResult> DeliveryResumeDownload([FromRoute] string campaignId, [FromRoute] string recruitmentId, [FromRoute] string openingId,  [FromRoute] string resumeId)
        {
            var act = await _dbContext.RecruitmentCampaigns
                .AsNoTracking()
                .Include(x => x.RecruitmentCampaignOpenings.Where(y => y.RecruitmentCampaignOpeningId == openingId))
                .ThenInclude(x => x.RecruitmentCampaignResumes)
                .ThenInclude(x => x.FileResume)
                .SingleOrDefaultAsync(x => x.CampaignId == campaignId && x.RecruitmentCampaignId == recruitmentId);
            var opening = act.RecruitmentCampaignOpenings.First();
            var resume = opening.RecruitmentCampaignResumes
                .SingleOrDefault(x => x.FileResumeId == resumeId);
            if (resume == null)
            {
                return NotFound();
            }
            // 路徑、型態、下載的名稱
            return File(System.IO.File.OpenRead(resume.FileResume.Path), resume.FileResume.Type, $"{resume.FileResume.Name}{resume.FileResume.Extension}");
        }
    }
}
