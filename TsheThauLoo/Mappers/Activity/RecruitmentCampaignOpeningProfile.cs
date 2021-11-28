using AutoMapper;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;
using TsheThauLoo.Entities.Job;
using TsheThauLoo.Entities.Resume;

namespace TsheThauLoo.Mappers.Activity
{
    public class RecruitmentCampaignOpeningProfile : Profile
    {
        public RecruitmentCampaignOpeningProfile()
        {
            #region RecruitmentCampaignOpeningCreateDto 轉換成 RecruitmentCampaignOpening

            CreateMap<RecruitmentCampaignOpeningCreateDto, RecruitmentCampaignOpening>()
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.JobDescription,
                    opt => opt.MapFrom(src => src.JobDescription))
                .ForMember(dest => dest.WorkPlace,
                    opt => opt.MapFrom(src => src.WorkPlace))
                .ForMember(dest => dest.Salary,
                    opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.RequiredNumber,
                    opt => opt.MapFrom(src => src.RequiredNumber))
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education))
                .ForMember(dest => dest.WorkExperience,
                    opt => opt.MapFrom(src => src.WorkExperience))
                .ForMember(dest => dest.Language,
                    opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Nationality,
                    opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.IsAccessibility,
                    opt => opt.MapFrom(src => src.IsAccessibility));

            #endregion
            
            #region RecruitmentCampaignOpeningEditDto 轉換成 RecruitmentCampaignOpening

            CreateMap<RecruitmentCampaignOpeningEditDto, RecruitmentCampaignOpening>()
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.JobDescription,
                    opt => opt.MapFrom(src => src.JobDescription))
                .ForMember(dest => dest.WorkPlace,
                    opt => opt.MapFrom(src => src.WorkPlace))
                .ForMember(dest => dest.Salary,
                    opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.RequiredNumber,
                    opt => opt.MapFrom(src => src.RequiredNumber))
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education))
                .ForMember(dest => dest.WorkExperience,
                    opt => opt.MapFrom(src => src.WorkExperience))
                .ForMember(dest => dest.Language,
                    opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Nationality,
                    opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.IsAccessibility,
                    opt => opt.MapFrom(src => src.IsAccessibility));

            #endregion
            
            #region RecruitmentCampaignOpening 轉換成 RecruitmentCampaignOpeningDto

            CreateMap<RecruitmentCampaignOpening, RecruitmentCampaignOpeningDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.RecruitmentCampaignOpeningId))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.JobDescription,
                    opt => opt.MapFrom(src => src.JobDescription))
                .ForMember(dest => dest.WorkPlace,
                    opt => opt.MapFrom(src => src.WorkPlace))
                .ForMember(dest => dest.Salary,
                    opt => opt.MapFrom(src => src.Salary))
                .ForMember(dest => dest.RequiredNumber,
                    opt => opt.MapFrom(src => src.RequiredNumber))
                .ForMember(dest => dest.Education,
                    opt => opt.MapFrom(src => src.Education))
                .ForMember(dest => dest.WorkExperience,
                    opt => opt.MapFrom(src => src.WorkExperience))
                .ForMember(dest => dest.Language,
                    opt => opt.MapFrom(src => src.Language))
                .ForMember(dest => dest.Nationality,
                    opt => opt.MapFrom(src => src.Nationality))
                .ForMember(dest => dest.IsAccessibility,
                    opt => opt.MapFrom(src => src.IsAccessibility))
                .ForPath(dest => dest.Qualifications,
                    opt => opt.MapFrom(src => src.Qualifications))
                .ForPath(dest => dest.Faculties,
                    opt => opt.MapFrom(src => src.Faculties));

            #endregion
            
            #region FacultyCreateDto 轉換成 Faculty

            CreateMap<FacultyCreateDto, Faculty>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
            
            #region FacultyEditDto 轉換成 Faculty

            CreateMap<FacultyEditDto, Faculty>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
            
            #region Faculty 轉換成 FacultyDto

            CreateMap<Faculty, FacultyDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.FacultyId))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
            
            #region QualificationCreateDto 轉換成 Qualification

            CreateMap<QualificationCreateDto, Qualification>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
            
            #region QualificationEditDto 轉換成 Qualification

            CreateMap<QualificationEditDto, Qualification>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
            
            #region Qualification 轉換成 QualificationDto

            CreateMap<Qualification, QualificationDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.QualificationId))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion

            #region RecruitmentCampaignResume 轉換成 ResumeDeliveryListDto

            CreateMap<RecruitmentCampaignResume, ResumeDeliveryListDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.RecruitmentCampaignResumeId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.IsInterview,
                    opt => opt.MapFrom(src => src.IsInterview))
                .ForMember(dest => dest.IsHire,
                    opt => opt.MapFrom(src => src.IsHire))
                .ForPath(dest => dest.Resume,
                    opt => opt.MapFrom(src => src.FileResume));

            #endregion
        }
    }
}