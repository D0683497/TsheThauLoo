using AutoMapper;
using TsheThauLoo.Dtos.Account.Profile.Student;
using TsheThauLoo.Dtos.Account.Register;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers.Account
{
    public class StudentProfile : Profile
    {
        public StudentProfile()
        {
            #region StudentRegisterDto 轉換成 ApplicationUser

            CreateMap<StudentRegisterDto, ApplicationUser>()
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
                .ForPath(dest => dest.Student.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForPath(dest => dest.Student.College,
                    opt => opt.MapFrom(src => src.College))
                .ForPath(dest => dest.Student.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForPath(dest => dest.Student.Class,
                    opt => opt.MapFrom(src => src.Class))
                .AfterMap((src, dest) =>
                {
                    dest.Student.ApplicationUserId = dest.Id;
                    dest.Student.ApplicationUser = dest;
                    dest.NationalVerify = new NationalVerify
                    {
                        ApplicationUserId = dest.Id
                    };
                    dest.Student.StudentVerify = new StudentVerify
                    {
                        StudentId = dest.Student.StudentId
                    };
                });


            #endregion
            
            #region ApplicationUser 轉換成 StudentProfileDto

            CreateMap<ApplicationUser, StudentProfileDto>()
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
                .ForPath(dest => dest.StudentConfirmed,
                    opt => opt.MapFrom(src => src.Student.StudentConfirmed))
                .ForPath(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.Student.NetworkId))
                .ForPath(dest => dest.College,
                    opt => opt.MapFrom(src => src.Student.College))
                .ForPath(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Student.Department))
                .ForPath(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Student.Class))
                .AfterMap((src, dest) =>
                {
                    dest.HasPhoto = src.UserPhoto != null;
                });
            
            #endregion
            
            #region Student 轉換成 StudentInfoDto

            CreateMap<Student, StudentInfoDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.StudentConfirmed,
                    opt => opt.MapFrom(src => src.StudentConfirmed))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class));
            
            #endregion
            
            #region StudentEditInfoDto 轉換成 Student

            CreateMap<StudentEditInfoDto, Student>()
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId.ToUpper()))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class));
            
            #endregion

            #region Student 轉換成 StudentVerifyDto

            CreateMap<Student, StudentVerifyDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.ApplicationUserId))
                .ForMember(dest => dest.StudentConfirmed,
                    opt => opt.MapFrom(src => src.StudentConfirmed))
                .ForMember(dest => dest.NetworkId,
                    opt => opt.MapFrom(src => src.NetworkId))
                .ForMember(dest => dest.College,
                    opt => opt.MapFrom(src => src.College))
                .ForMember(dest => dest.Department,
                    opt => opt.MapFrom(src => src.Department))
                .ForMember(dest => dest.Class,
                    opt => opt.MapFrom(src => src.Class))
                .ForPath(dest => dest.Description,
                    opt => opt.MapFrom(src => src.StudentVerify.Description))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.StudentVerify.StudentVerifyFiles));

            #endregion

            #region StudentEditVerifyDto 轉換成 StudentVerify

            CreateMap<StudentEditVerifyDto, StudentVerify>()
                .ForMember(dest => dest.Description,
                    opt => opt.MapFrom(src => src.Description));

            #endregion
        }
    }
}