using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Examiner;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class ExaminerProfile : Profile
    {
        public ExaminerProfile()
        {
            #region ExaminerRegisterDto 轉換成 ApplicationUser

            CreateMap<ExaminerRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Examiner.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForPath(dest => dest.Examiner.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle))
                .AfterMap((src, dest) =>
                {
                    dest.Examiner.ApplicationUserId = dest.Id;
                    dest.Examiner.ApplicationUser = dest;
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                });

            #endregion
            
            #region ApplicationUser 轉換成 ExaminerProfileDto

            CreateMap<ApplicationUser, ExaminerProfileDto>()
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
                .ForPath(dest => dest.ExaminerConfirmed,
                    opt => opt.MapFrom(src => src.Examiner.ExaminerConfirmed))
                .ForPath(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.Examiner.DivisionName))
                .ForPath(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.Examiner.JobTitle))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });

            #endregion
            
            #region Examiner 轉換成 ExaminerInfoDto

            CreateMap<Examiner, ExaminerInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.ExaminerConfirmed,
                    opt => opt.MapFrom(src => src.ExaminerConfirmed))
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle));

            #endregion
            
            #region ExaminerEditInfoDto 轉換成 Examiner

            CreateMap<ExaminerEditInfoDto, Examiner>()
                .ForMember(dest => dest.DivisionName,
                    opt => opt.MapFrom(src => src.DivisionName))
                .ForMember(dest => dest.JobTitle,
                    opt => opt.MapFrom(src => src.JobTitle));

            #endregion
        }
    }
}