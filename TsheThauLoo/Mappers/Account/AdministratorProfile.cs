using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Administrator;
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
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                });

            #endregion
            
            #region ApplicationUser 轉換成 AdministratorProfileDto
            
            CreateMap<ApplicationUser, AdministratorProfileDto>()
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
                .ForPath(dest => dest.AdministratorConfirmed,
                    opt => opt.MapFrom(src => src.Administrator.AdministratorConfirmed))
                .ForPath(dest => dest.ShowAbout,
                    opt => opt.MapFrom(src => src.Administrator.ShowAbout))
                .ForPath(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.Administrator.NetworkId))
                .ForPath(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Administrator.Dept))
                .ForPath(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Administrator.Unit))
                .ForPath(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Administrator.JobTitle))
                .ForPath(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Administrator.Extension))
                .ForPath(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.Administrator.ContactEmail))
                .ForPath(dest => dest.Responsibilities,
                    opt => opt.MapFrom(src => src.Administrator.Responsibilities))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });
            
            #endregion

            #region Responsibility 轉換成 ResponsibilityDto

            CreateMap<Responsibility, ResponsibilityDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ResponsibilityId))
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion

            #region Administrator 轉換成 AdministratorInfoDto

            CreateMap<Administrator, AdministratorInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.AdministratorConfirmed,
                    opt => opt.MapFrom(src => src.AdministratorConfirmed))
                .ForMember(dest => dest.ShowAbout,
                    opt => opt.MapFrom(src => src.ShowAbout))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail))
                .ForPath(dest => dest.Responsibilities,
                    opt => opt.MapFrom(src => src.Responsibilities));

            #endregion
            
            #region AdministratorEditInfoDto 轉換成 Administrator

            CreateMap<AdministratorEditInfoDto, Administrator>()
                .ForMember(dest => dest.ShowAbout,
                    opt => opt.MapFrom(src => src.ShowAbout))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForMember(dest => dest.Dept,
                    opt => opt.MapFrom(src => src.Dept))
                .ForMember(dest => dest.Unit,
                    opt => opt.MapFrom(src => src.Unit))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension))
                .ForMember(dest => dest.ContactEmail,
                    opt => opt.MapFrom(src => src.ContactEmail));

            #endregion

            #region CreateResponsibilityDto 轉換成 Responsibility

            CreateMap<ResponsibilityCreateDto, Responsibility>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion

            #region EditResponsibilityDto 轉換成 Responsibility

            CreateMap<ResponsibilityEditDto, Responsibility>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
        }
    }
}