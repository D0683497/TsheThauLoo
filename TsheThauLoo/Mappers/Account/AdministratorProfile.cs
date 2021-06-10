using AutoMapper;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class AdministratorProfile : Profile
    {
        public AdministratorProfile()
        {
            #region AdministratorRegisterDto 轉換成 ApplicationUser

            CreateMap<AdministratorRegisterDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.NationalId,
                    opt => opt.MapFrom(src => src.NationalId.ToUpper()))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.CurrentAddress,
                    opt => opt.MapFrom(src => src.CurrentAddress))
                .ForPath(dest => dest.Administrator.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForPath(dest => dest.Administrator.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForPath(dest => dest.Administrator.Unit,
                    opt => opt.MapFrom(src => src.Unit))
                .ForPath(dest => dest.Administrator.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForPath(dest => dest.Administrator.Extension,
                    opt => opt.MapFrom(src => src.Extension))
                .ForPath(dest => dest.Administrator.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .AfterMap((src, dest) =>
                {
                    dest.Administrator.ApplicationUserId = dest.Id;
                    dest.Administrator.ApplicationUser = dest;
                });

            #endregion
        }
    }
}