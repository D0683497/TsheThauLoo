using AutoMapper;
using TsheThauLoo.Dtos.Account.Login;
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
            
            #region NIDUserInfoDto 轉換成 ApplicationUser

            CreateMap<NIDUserInfoDto, ApplicationUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.EmailConfirmed,
                    opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.IsEnable,
                    opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .AfterMap((src, dest) =>
                {
                    switch (src.Type)
                    {
                        case "學生":
                            dest.Student = new Student
                            {
                                StudentConfirmed = true,
                                NetworkId = src.Id,
                                College = src.DeptName,
                                Department = src.UnitName,
                                Class = src.ClassName.Split(src.UnitName)[1],
                                ApplicationUserId = dest.Id,
                                ApplicationUser = dest
                            };
                            dest.Student.StudentVerify = new StudentVerify
                            {
                                StudentId = dest.Student.StudentId
                            };
                            break;
                        case "教職員工":
                            dest.Employee = new Employee
                            {
                                EmployeeConfirmed = true,
                                NetworkId = src.Id,
                                Dept = src.DeptName,
                                Unit = src.UnitName,
                                ApplicationUserId = dest.Id,
                                ApplicationUser = dest
                            };
                            break;
                    }
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                });

            #endregion
        }
    }
}