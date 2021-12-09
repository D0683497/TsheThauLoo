using AutoMapper;
using TsheThauLoo.Entities.School;
using TsheThauLoo.Models.School;

namespace TsheThauLoo.Mappers
{
    public class SchoolProfile : Profile
    {
        public SchoolProfile()
        {
            CreateMap<College, CollegeDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name));

            CreateMap<Department, DepartmentDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Degree,
                    opt => opt.MapFrom(src => src.Degree));
        }
    }
}
