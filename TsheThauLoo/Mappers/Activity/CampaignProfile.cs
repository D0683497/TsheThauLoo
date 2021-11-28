using System;
using AutoMapper;
using TsheThauLoo.Dtos.Activity.Campaign;
using TsheThauLoo.Dtos.Activity.MyCampaign;
using TsheThauLoo.Entities.Activity;

namespace TsheThauLoo.Mappers.Activity
{
    public class CampaignProfile : Profile
    {
        public CampaignProfile()
        {
            #region CampaignCreateDto 轉換成 Campaign

            CreateMap<CampaignCreateDto, Campaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion
            
            #region Campaign 轉換成 CampaignDto

            CreateMap<Campaign, CampaignDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CampaignId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.CampaignFiles));

            #endregion
            
            #region CampaignEditDto 轉換成 Campaign

            CreateMap<CampaignEditDto, Campaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion
            
            #region Campaign 轉換成 MyCampaignDto

            CreateMap<Campaign, MyCampaignDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.CampaignId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForPath(dest => dest.GeneralCampaigns,
                    opt => opt.MapFrom(src => src.GeneralCampaigns));

            #endregion
        }
    }
}