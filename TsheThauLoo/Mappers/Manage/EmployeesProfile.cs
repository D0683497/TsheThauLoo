using AutoMapper;
using TsheThauLoo.Dtos.Manage;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Manage
{
    public class EmployeesProfile : Profile
    {
        public EmployeesProfile()
        {
            #region Employee 轉換成 EmployeeDto

            CreateMap<Employee, EmployeeDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForPath(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.ApplicationUser.UserName))
                .ForPath(dest => dest.Email,
                    opt => opt.MapFrom(src => src.ApplicationUser.Email))
                .ForPath(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => src.ApplicationUser.EmailConfirmed))
                .ForPath(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumber))
                .ForPath(dest => dest.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.ApplicationUser.PhoneNumberConfirmed))
                .ForPath(dest => dest.LockoutEnd,
                    opt => opt.MapFrom(src => src.ApplicationUser.LockoutEnd))
                .ForPath(dest => dest.LockoutEnabled,
                    opt => opt.MapFrom(src => src.ApplicationUser.LockoutEnabled))
                .ForPath(dest => dest.AccessFailedCount,
                    opt => opt.MapFrom(src => src.ApplicationUser.AccessFailedCount))
                .ForPath(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => src.ApplicationUser.IsEnable))
                .ForPath(dest => dest.IdentityConfirmed,
                    opt => opt.MapFrom(src => src.ApplicationUser.IdentityConfirmed))
                .ForPath(dest => dest.NationalId,
                    opt => opt.MapFrom(src => src.ApplicationUser.NationalId))
                .ForPath(dest => dest.Name,
                    opt => opt.MapFrom(src => src.ApplicationUser.Name))
                .ForPath(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.ApplicationUser.Gender))
                .ForPath(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.ApplicationUser.DateOfBirth))
                .ForPath(dest => dest.CurrentAddress,
                    opt => opt.MapFrom(src => src.ApplicationUser.CurrentAddress))
                .ForMember(dest => dest.EmployeeConfirmed,
                    opt => opt.MapFrom(src => src.EmployeeConfirmed))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.ApplicationUser.UserPhoto != null;
                });

            #endregion

            #region EmployeeManageDto 轉換成 Employee

            CreateMap<EmployeeManageDto, Employee>()
                .ForPath(dest => dest.ApplicationUser.UserName,
                    opt => opt.MapFrom(src => src.UserName))
                .ForPath(dest => dest.ApplicationUser.NormalizedUserName,
                    opt => opt.MapFrom(src => src.UserName.ToUpper()))
                .ForPath(dest => dest.ApplicationUser.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForPath(dest => dest.ApplicationUser.NormalizedEmail,
                    opt => opt.MapFrom(src => src.Email.ToUpper()))
                .ForPath(dest => dest.ApplicationUser.EmailConfirmed,
                    opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForPath(dest => dest.ApplicationUser.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ForPath(dest => dest.ApplicationUser.PhoneNumberConfirmed,
                    opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForPath(dest => dest.ApplicationUser.LockoutEnd,
                    opt => opt.MapFrom(src => src.LockoutEnd))
                .ForPath(dest => dest.ApplicationUser.LockoutEnabled,
                    opt => opt.MapFrom(src => src.LockoutEnabled))
                .ForPath(dest => dest.ApplicationUser.AccessFailedCount,
                    opt => opt.MapFrom(src => src.AccessFailedCount))
                .ForPath(dest => dest.ApplicationUser.IsEnable,
                    opt => opt.MapFrom(src => src.IsEnable))
                .ForPath(dest => dest.ApplicationUser.IdentityConfirmed,
                    opt => opt.MapFrom(src => src.IdentityConfirmed))
                .ForPath(dest => dest.ApplicationUser.NationalId,
                    opt => opt.MapFrom(src => src.NationalId))
                .ForPath(dest => dest.ApplicationUser.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForPath(dest => dest.ApplicationUser.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForPath(dest => dest.ApplicationUser.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForPath(dest => dest.ApplicationUser.CurrentAddress,
                    opt => opt.MapFrom(src => src.CurrentAddress))
                .ForMember(dest => dest.EmployeeConfirmed,
                    opt => opt.MapFrom(src => src.EmployeeConfirmed))
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