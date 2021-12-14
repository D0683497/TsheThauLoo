using AutoMapper;
using TsheThauLoo.Entities.Identity;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Models.Account;

namespace TsheThauLoo.Mappers;

public class AccountProfile : Profile
{
    public AccountProfile()
    {
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.NationalId,
                opt => opt.MapFrom(src => src.NationalId))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address));

        CreateMap<AlumnusRegisterDto, ApplicationUser>()
            .ForPath(dest => dest.Alumnus!.DateOfGraduation,
                opt => opt.MapFrom(src => src.DateOfGraduation))
            .ForPath(dest => dest.Alumnus!.College,
                opt => opt.MapFrom(src => src.College))
            .ForPath(dest => dest.Alumnus!.Department,
                opt => opt.MapFrom(src => src.Department))
            .IncludeBase<RegisterDto, ApplicationUser>()
            .AfterMap((src, dest) =>
            {
                dest.LockoutEnd = DateTimeOffset.MaxValue;
            });

        CreateMap<EmployeeRegisterDto, ApplicationUser>()
            .ForPath(dest => dest.Employee!.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForPath(dest => dest.Employee!.Division,
                opt => opt.MapFrom(src => src.Division))
            .ForPath(dest => dest.Employee!.Substitute,
                opt => opt.MapFrom(src => src.Substitute))
            .IncludeBase<RegisterDto, ApplicationUser>()
            .AfterMap((src, dest) =>
            {
                dest.LockoutEnd = DateTimeOffset.MaxValue;
            });

        CreateMap<SubstituteRegisterDto, Substitute>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Division,
                opt => opt.MapFrom(src => src.Division));

        CreateMap<ApplicationUser, ProfileDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.UserName,
                opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.NationalId,
                opt => opt.MapFrom(src => src.NationalId))
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Gender,
                opt => opt.MapFrom(src => src.Gender))
            .ForMember(dest => dest.DateOfBirth,
                opt => opt.MapFrom(src => src.DateOfBirth))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address));

        CreateMap<Alumnus, AlumnusProfileDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DateOfGraduation,
                opt => opt.MapFrom(src => src.DateOfGraduation))
            .ForMember(dest => dest.College,
                opt => opt.MapFrom(src => src.College))
            .ForMember(dest => dest.Department,
                opt => opt.MapFrom(src => src.Department));

        CreateMap<Employee, EmployeeProfileDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Division,
                opt => opt.MapFrom(src => src.Division))
            .ForMember(dest => dest.Substitute,
                opt => opt.MapFrom(src => src.Substitute));
        
        CreateMap<Substitute, SubstituteProfileDto>()
            .ForMember(dest => dest.Name,
                opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.Email,
                opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber,
                opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Address,
                opt => opt.MapFrom(src => src.Address))
            .ForMember(dest => dest.Title,
                opt => opt.MapFrom(src => src.Title))
            .ForMember(dest => dest.Division,
                opt => opt.MapFrom(src => src.Division));

        CreateMap<Staff, StaffProfileDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NetworkId,
                opt => opt.MapFrom(src => src.NetworkId))
            .ForMember(dest => dest.Dept,
                opt => opt.MapFrom(src => src.Dept))
            .ForMember(dest => dest.Unit,
                opt => opt.MapFrom(src => src.Unit));

        CreateMap<Student, StudentProfileDto>()
            .ForMember(dest => dest.Id,
                opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.NetworkId,
                opt => opt.MapFrom(src => src.NetworkId))
            .ForMember(dest => dest.College,
                opt => opt.MapFrom(src => src.College))
            .ForMember(dest => dest.Department,
                opt => opt.MapFrom(src => src.Department));
    }
}