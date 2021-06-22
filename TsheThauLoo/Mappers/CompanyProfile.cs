using AutoMapper;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Entities.Business;

namespace TsheThauLoo.Mappers
{
    public class CompanyProfile : Profile
    {
        public CompanyProfile()
        {
            #region Company 轉換成 CompanyDto

            CreateMap<Company, CompanyDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CompanyId))
                .ForMember(dest => dest.CompanyConfirmed,
                    opt => opt.MapFrom(src => src.CompanyConfirmed))
                .ForMember(dest => dest.RegistrationNumber,
                    opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Introduction,
                    opt => opt.MapFrom(src => src.Introduction))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website))
                .ForPath(dest => dest.IndustrialClassifications,
                    opt => opt.MapFrom(src => src.IndustrialClassifications))
                .AfterMap((src, dest) =>
                {
                    dest.HasLogo = src.CompanyLogo != null;
                });

            #endregion

            #region IndustrialClassification 轉換成 IndustrialClassificationDto

            CreateMap<IndustrialClassification, IndustrialClassificationDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.IndustrialClassificationId))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion

            #region CompanyCreateDto 轉換成 Company

            CreateMap<CompanyCreateDto, Company>()
                .ForMember(dest => dest.RegistrationNumber,
                    opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Introduction,
                    opt => opt.MapFrom(src => src.Introduction))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website));

            #endregion

            #region CompanyEditDto 轉換成 Company

            CreateMap<CompanyEditDto, Company>()
                .ForMember(dest => dest.RegistrationNumber,
                    opt => opt.MapFrom(src => src.RegistrationNumber))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Introduction,
                    opt => opt.MapFrom(src => src.Introduction))
                .ForMember(dest => dest.Website,
                    opt => opt.MapFrom(src => src.Website));

            #endregion

            #region IndustrialClassificationCreateDto 轉換成 IndustrialClassification

            CreateMap<IndustrialClassificationCreateDto, IndustrialClassification>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion

            #region IndustrialClassificationEditDto 轉換成 IndustrialClassification

            CreateMap<IndustrialClassificationEditDto, IndustrialClassification>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
        }
    }
}