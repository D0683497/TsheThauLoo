using System;
using AutoMapper;
using TsheThauLoo.Dtos.Activity.RecruitmentCampaign;
using TsheThauLoo.Entities.Activity;

namespace TsheThauLoo.Mappers.Activity
{
    public class RecruitmentCampaignProfile : Profile
    {
        public RecruitmentCampaignProfile()
        {
            #region RecruitmentCampaignCreateDto 轉換成 RecruitmentCampaign

            CreateMap<RecruitmentCampaignCreateDto, RecruitmentCampaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EnableReview,
                    opt => opt.MapFrom(src => src.EnableReview))
                .AfterMap((src, dest) =>
                {
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion

            #region RecruitmentCampaign 轉換成 RecruitmentCampaignDto

            CreateMap<RecruitmentCampaign, RecruitmentCampaignDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.RecruitmentCampaignId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.RecruitmentCampaignFiles))
                .ForPath(dest => dest.Company,
                    opt => opt.MapFrom(src => src.Company));

            #endregion
            
            #region RecruitmentCampaignEditDto 轉換成 RecruitmentCampaign

            CreateMap<RecruitmentCampaignEditDto, RecruitmentCampaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EnableReview,
                    opt => opt.MapFrom(src => src.EnableReview))
                .AfterMap((src, dest) =>
                {
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion
        }
    }
}