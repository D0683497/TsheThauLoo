using System.IO;
using AutoMapper;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            #region StudentVerifyFile

            #region StudentVerifyFile 轉換成 FileDto

            CreateMap<StudentVerifyFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.StudentVerifyFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #region FileCreateDto 轉換成 StudentVerifyFile

            CreateMap<FileCreateDto, StudentVerifyFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"users{Path.DirectorySeparatorChar}"+
                                $"verify{Path.DirectorySeparatorChar}" +
                                $"student{Path.DirectorySeparatorChar}" +
                                $"{Path.GetRandomFileName()}";
                });

            #endregion

            #region FileEditDto 轉換成 StudentVerifyFile

            CreateMap<FileEditDto, StudentVerifyFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region AlumnusVerifyFile

            #region AlumnusVerifyFile 轉換成 FileDto

            CreateMap<AlumnusVerifyFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.AlumnusVerifyFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #region FileCreateDto 轉換成 AlumnusVerifyFile

            CreateMap<FileCreateDto, AlumnusVerifyFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"users{Path.DirectorySeparatorChar}"+
                                $"verify{Path.DirectorySeparatorChar}" +
                                $"alumnus{Path.DirectorySeparatorChar}" +
                                $"{Path.GetRandomFileName()}";
                });

            #endregion

            #region FileEditDto 轉換成 AlumnusVerifyFile

            CreateMap<FileEditDto, AlumnusVerifyFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region NationalVerifyFile

            #region NationalVerifyFile 轉換成 FileDto

            CreateMap<NationalVerifyFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.NationalVerifyFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #region FileCreateDto 轉換成 NationalVerifyFile

            CreateMap<FileCreateDto, NationalVerifyFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"users{Path.DirectorySeparatorChar}"+
                                $"verify{Path.DirectorySeparatorChar}" +
                                $"national{Path.DirectorySeparatorChar}" +
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileEditDto 轉換成 NationalVerifyFile

            CreateMap<FileEditDto, NationalVerifyFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region UserPhoto

            #region FileCreateDto 轉換成 UserPhoto

            CreateMap<FileCreateDto, UserPhoto>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"users{Path.DirectorySeparatorChar}"+
                                $"photo{Path.DirectorySeparatorChar}" +
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region UserPhoto 轉換成 FileDto

            CreateMap<UserPhoto, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.UserPhotoId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region CompanyLogo

            #region FileCreateDto 轉換成 CompanyLogo

            CreateMap<FileCreateDto, CompanyLogo>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"companies{Path.DirectorySeparatorChar}"+
                                $"logo{Path.DirectorySeparatorChar}" +
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region CompanyLogo 轉換成 FileDto

            CreateMap<CompanyLogo, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CompanyLogoId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion
            
            #region FileResume

            #region FileCreateDto 轉換成 FileResume

            CreateMap<FileCreateDto, FileResume>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"resumes{Path.DirectorySeparatorChar}"+
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileResume 轉換成 FileDto

            CreateMap<FileResume, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.FileResumeId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion
            
            #region FileEditDto 轉換成 FileResume

            CreateMap<FileEditDto, FileResume>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region EventFile

            #region EventFile 轉換成 FileDto

            CreateMap<EventFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.EventFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion
            
            #region FileCreateDto 轉換成 EventFile

            CreateMap<FileCreateDto, EventFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"activities{Path.DirectorySeparatorChar}"+
                                $"event{Path.DirectorySeparatorChar}"+
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileEditDto 轉換成 EventFile

            CreateMap<FileEditDto, EventFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region CampaignFile

            #region CampaignFile 轉換成 FileDto

            CreateMap<CampaignFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CampaignFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion
            
            #region FileCreateDto 轉換成 CampaignFile

            CreateMap<FileCreateDto, CampaignFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"activities{Path.DirectorySeparatorChar}"+
                                $"campaign{Path.DirectorySeparatorChar}"+
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileEditDto 轉換成 CampaignFile

            CreateMap<FileEditDto, CampaignFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion

            #region GeneralCampaignFile

            #region GeneralCampaignFile 轉換成 FileDto

            CreateMap<GeneralCampaignFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.GeneralCampaignFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion
            
            #region FileCreateDto 轉換成 GeneralCampaignFile

            CreateMap<FileCreateDto, GeneralCampaignFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"activities{Path.DirectorySeparatorChar}"+
                                $"general-campaign{Path.DirectorySeparatorChar}"+
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileEditDto 轉換成 CampaignFile

            CreateMap<FileEditDto, GeneralCampaignFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion
            
            #region RecruitmentCampaignFile

            #region RecruitmentCampaignFile 轉換成 FileDto

            CreateMap<RecruitmentCampaignFile, FileDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.RecruitmentCampaignFileId))
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion
            
            #region FileCreateDto 轉換成 RecruitmentCampaignFile

            CreateMap<FileCreateDto, RecruitmentCampaignFile>()
                .ForMember(dest => dest.Type,
                    opt => opt.MapFrom(src => src.Type))
                .AfterMap((src, dest) =>
                {
                    var name = Path.GetFileNameWithoutExtension(src.Name);
                    dest.Name = name;
                    var extension = Path.GetExtension(src.Name);
                    dest.Extension = extension == string.Empty ? null : extension;
                    dest.Path = $@"wwwroot{Path.DirectorySeparatorChar}"+
                                $"activities{Path.DirectorySeparatorChar}"+
                                $"recruitment-campaign{Path.DirectorySeparatorChar}"+
                                $"{Path.GetRandomFileName()}";
                });

            #endregion
            
            #region FileEditDto 轉換成 RecruitmentCampaignFile

            CreateMap<FileEditDto, RecruitmentCampaignFile>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Extension,
                    opt => opt.MapFrom(src => src.Extension));

            #endregion

            #endregion
        }
    }
}
