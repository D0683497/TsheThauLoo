using AutoMapper;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class EmployeeProfile : Profile
    {
        public EmployeeProfile()
        {
            #region EmployeeRegisterDto 轉換成 ApplicationUser

            CreateMap<EmployeeRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Employee.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForPath(dest => dest.Employee.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForPath(dest => dest.Employee.Unit,
                    opt => opt.MapFrom(src => src.Unit))
                .AfterMap((src, dest) =>
                {
                    dest.Employee.ApplicationUserId = dest.Id;
                    dest.Employee.ApplicationUser = dest;
                });

            #endregion
        }
    }
}