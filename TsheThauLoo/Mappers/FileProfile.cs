using System;
using System.IO;
using AutoMapper;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Mappers
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
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
        }
    }
}