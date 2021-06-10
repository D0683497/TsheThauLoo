using AutoMapper;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            #region StudentRegisterDto 轉換成 ApplicationUser

            CreateMap<StudentRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Student.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForPath(dest => dest.Student.College,
                    opt => opt.MapFrom(src => src.College))
                .ForPath(dest => dest.Student.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForPath(dest => dest.Student.Class,
                    opt => opt.MapFrom(src => src.Class))
                .AfterMap((src, dest) =>
                {
                    dest.Student.ApplicationUserId = dest.Id;
                    dest.Student.ApplicationUser = dest;
                });


            #endregion
        }
    }
}