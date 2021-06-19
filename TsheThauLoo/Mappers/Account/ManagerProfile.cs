using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Manager;
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
                .ForPath(dest => dest.Manager.Substitute,
                    opt => opt.MapFrom(src => src.Substitute))
                .AfterMap((src, dest) =>
                {
                    dest.Manager.ApplicationUserId = dest.Id;
                    dest.Manager.ApplicationUser = dest;
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                    dest.Manager.Substitute.ManagerId = dest.Manager.ManagerId;
                });

            #endregion

            #region SubstituteRegisterDto 轉換成 Substitute

            CreateMap<SubstituteRegisterDto, Substitute>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress));

            #endregion
            
            #region ApplicationUser 轉換成 ManagerProfileDto

            CreateMap<ApplicationUser, ManagerProfileDto>()
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
                .ForPath(dest => dest.ManagerConfirmed,
                    opt => opt.MapFrom(src => src.Manager.ManagerConfirmed))
                .ForPath(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.Manager.DivisionName))
                .ForPath(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Manager.JobTitle))
                .ForPath(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.Manager.ContactEmail))
                .ForPath(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.Manager.ContactPhone))
                .ForPath(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.Manager.ContactAddress))
                .ForPath(dest => dest.Substitute,
                    opt => opt.MapFrom(src => src.Manager.Substitute))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });

            #endregion

            #region Substitute 轉換成 SubstituteDto

            CreateMap<Substitute, SubstituteDto>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress));

            #endregion
            
            #region Manager 轉換成 ManagerInfoDto

            CreateMap<Manager, ManagerInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.ManagerConfirmed,
                    opt => opt.MapFrom(src => src.ManagerConfirmed))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress))
                .ForPath(dest => dest.Substitute,
                    opt => opt.MapFrom(src => src.Substitute));

            #endregion
            
            #region ManagerEditInfoDto 轉換成 Manager

            CreateMap<ManagerEditInfoDto, Manager>()
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress));

            #endregion
            
            #region SubstituteEditDto 轉換成 Substitute

            CreateMap<SubstituteEditDto, Substitute>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.ContactAddress,
                    opt => opt.MapFrom(src => src.ContactAddress));

            #endregion
        }
    }
}