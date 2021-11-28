using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Administrator;
using TsheThauLoo.Dtos.Account.Profile.Alumnus;
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
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                    dest.Alumnus.AlumnusVerify = new AlumnusVerify
                    {
                        AlumnusId = dest.Alumnus.AlumnusId
                    };
                });

            #endregion
            
            #region ApplicationUser 轉換成 AlumnusProfileDto

            CreateMap<ApplicationUser, AlumnusProfileDto>()
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
                .ForPath(dest => dest.AlumnusConfirmed,
                    opt => opt.MapFrom(src => src.Alumnus.AlumnusConfirmed))
                .ForPath(dest => dest.DateOfGraduation,
                    opt => opt.MapFrom(src => src.Alumnus.DateOfGraduation))
                .ForPath(dest => dest.College,
                    opt => opt.MapFrom(src => src.Alumnus.College))
                .ForPath(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Alumnus.Department))
                .ForPath(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Alumnus.Class))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });

            #endregion
            
            #region Alumnus 轉換成 AlumnusInfoDto

            CreateMap<Alumnus, AlumnusInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.AlumnusConfirmed,
                    opt => opt.MapFrom(src => src.AlumnusConfirmed))
                .ForMember(dest => dest.DateOfGraduation,
                    opt => opt.MapFrom(src => src.DateOfGraduation))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class));

            #endregion
            
            #region AlumnusEditInfoDto 轉換成 Alumnus

            CreateMap<AlumnusEditInfoDto, Alumnus>()
                .ForMember(dest => dest.DateOfGraduation,
                    opt => opt.MapFrom(src => src.DateOfGraduation))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class));

            #endregion
            
            #region Alumnus 轉換成 AlumnusVerifyDto

            CreateMap<Alumnus, AlumnusVerifyDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.AlumnusConfirmed,
                    opt => opt.MapFrom(src => src.AlumnusConfirmed))
                .ForMember(dest => dest.DateOfGraduation,
                    opt => opt.MapFrom(src => src.DateOfGraduation))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class))
                .ForPath(dest => dest.Description,
                    opt => opt.MapFrom(src => src.AlumnusVerify.Description))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.AlumnusVerify.AlumnusVerifyFiles));

            #endregion

            #region AlumnusEditVerifyDto 轉換成 AlumnusVerify

            CreateMap<AlumnusEditVerifyDto, AlumnusVerify>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
        }
    }
}