using AutoMapper;
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
                });

            #endregion
        }
    }
}