using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Employee;
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
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                });

            #endregion
            
            #region ApplicationUser 轉換成 EmployeeProfileDto

            CreateMap<ApplicationUser, EmployeeProfileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable))
                .ForMember(dest => dest.IdentityConfirmed,
                    opt => opt.MapFrom(src => src.IdentityConfirmed))
                .ForMember(dest => dest.NationalId,
                    opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.CurrentAddress,
                    opt => opt.MapFrom(src => src.CurrentAddress))
                .ForPath(dest => dest.EmployeeConfirmed,
                    opt => opt.MapFrom(src => src.Employee.EmployeeConfirmed))
                .ForPath(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.Employee.NetworkId))
                .ForPath(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Employee.Dept))
                .ForPath(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Employee.Unit))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });

            #endregion
            
            #region Employee 轉換成 EmployeeInfoDto

            CreateMap<Employee, EmployeeInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.EmployeeConfirmed,
                    opt => opt.MapFrom(src => src.EmployeeConfirmed))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit));

            #endregion
            
            #region EmployeeEditInfoDto 轉換成 Employee

            CreateMap<EmployeeEditInfoDto, Employee>()
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit));

            #endregion
        }
    }
}