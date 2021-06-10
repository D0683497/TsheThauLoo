using AutoMapper;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class ManagerProfile : Profile
    {
        public ManagerProfile()
        {
            #region ManagerRegisterDto 轉換成 ApplicationUser

            CreateMap<ManagerRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Manager.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForPath(dest => dest.Manager.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForPath(dest => dest.Manager.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForPath(dest => dest.Manager.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForPath(dest => dest.Manager.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress))
                .AfterMap((src, dest) =>
                {
                    dest.Manager.ApplicationUserId = dest.Id;
                    dest.Manager.ApplicationUser = dest;
                });

            #endregion
        }
    }
}