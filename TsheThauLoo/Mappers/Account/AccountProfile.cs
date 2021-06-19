using AutoMapper;
using TsheThauLoo.Dtos.Account.National;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class AccountProfile : Profile
    {
        public AccountProfile()
        {
            #region ApplicationUser 轉換成 NationalDto

            CreateMap<ApplicationUser, NationalDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
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
                    opt => opt.MapFrom(src => src.CurrentAddress));

            #endregion

            #region NationalEditDto 轉換成 ApplicationUser

            CreateMap<NationalEditDto, ApplicationUser>()
                .ForMember(dest => dest.NationalId,
                    opt => opt.MapFrom(src => src.NationalId.ToUpper()))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Gender,
                    opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.DateOfBirth,
                    opt => opt.MapFrom(src => src.DateOfBirth))
                .ForMember(dest => dest.CurrentAddress,
                    opt => opt.MapFrom(src => src.CurrentAddress));

            #endregion

            #region ApplicationUser 轉換成 NationalVerifyDto

            CreateMap<ApplicationUser, NationalVerifyDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
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
                .ForPath(dest => dest.Description,
                    opt => opt.MapFrom(src => src.NationalVerify.Description))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.NationalVerify.NationalVerifyFiles));

            #endregion

            #region NationalEditVerifyDto 轉換成 NationalVerify

            CreateMap<NationalEditVerifyDto, NationalVerify>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
        }
    }
}