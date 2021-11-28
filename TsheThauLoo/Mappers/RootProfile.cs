using AutoMapper;
using TsheThauLoo.Dtos;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers
{
    public class RootProfile : Profile
    {
        public RootProfile()
        {
            #region ApplicationUser 轉換成 AboutDto

            CreateMap<ApplicationUser, AboutDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Administrator.JobTitle))
                .ForPath(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Administrator.Extension))
                .ForPath(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.Administrator.ContactEmail))
                .ForPath(dest => dest.Responsibilities,
                    opt => opt.MapFrom(src => src.Administrator.Responsibilities));

            #endregion
        }
    }
}