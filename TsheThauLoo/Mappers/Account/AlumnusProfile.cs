using AutoMapper;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class AlumnusProfile : Profile
    {
        public AlumnusProfile()
        {
            #region AlumnusRegisterDto 轉換成 ApplicationUser

            CreateMap<AlumnusRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Alumnus.DateOfGraduation,
                    opt => opt.MapFrom(src => src.DateOfGraduation))
                .ForPath(dest => dest.Alumnus.College,
                    opt => opt.MapFrom(src => src.College))
                .ForPath(dest => dest.Alumnus.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForPath(dest => dest.Alumnus.Class,
                    opt => opt.MapFrom(src => src.Class))
                .AfterMap((src, dest) =>
                {
                    dest.Alumnus.ApplicationUserId = dest.Id;
                    dest.Alumnus.ApplicationUser = dest;
                });

            #endregion
        }
    }
}