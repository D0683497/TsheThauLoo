using System;
using System.Linq;
using AutoMapper;
using TsheThauLoo.Dtos.Activity.GeneralCampaign;
using TsheThauLoo.Dtos.Activity.MyCampaign;
using TsheThauLoo.Entities.Activity;

namespace TsheThauLoo.Mappers.Activity
{
    public class GeneralCampaignProfile : Profile
    {
        public GeneralCampaignProfile()
        {
            #region GeneralCampaignCreateDto 轉換成 GeneralCampaign

            CreateMap<GeneralCampaignCreateDto, GeneralCampaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Declaration,
                    opt => opt.MapFrom(src => src.Declaration))
                .ForMember(dest => dest.Venue,
                    opt => opt.MapFrom(src => src.Venue))
                .ForMember(dest => dest.RegistrationStartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationEndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LimitNumberOfPeople,
                    opt => opt.MapFrom(src => src.LimitNumberOfPeople))
                .ForMember(dest => dest.EnableVerify,
                    opt => opt.MapFrom(src => src.EnableVerify))
                .ForMember(dest => dest.EnableIdentityConfirmed,
                    opt => opt.MapFrom(src => src.EnableIdentityConfirmed))
                .AfterMap((src, dest) =>
                {
                    if (src.RegistrationStartDate != null && src.RegistrationStartTime != null)
                    {
                        var date = (DateTime) src.RegistrationStartDate;
                        var time = (DateTime) src.RegistrationStartTime;
                        dest.RegistrationStartTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                    }
                    else
                    {
                        dest.RegistrationStartTime = null;
                    }
                    if (src.RegistrationEndDate != null && src.RegistrationEndTime != null)
                    {
                        var date = (DateTime) src.RegistrationEndDate;
                        var time = (DateTime) src.RegistrationEndTime;
                        dest.RegistrationEndTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                    }
                    else
                    {
                        dest.RegistrationEndTime = null;
                    }
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion
            
            #region GeneralCampaign 轉換成 GeneralCampaignDto

            CreateMap<GeneralCampaign, GeneralCampaignDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.GeneralCampaignId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Declaration,
                    opt => opt.MapFrom(src => src.Declaration))
                .ForMember(dest => dest.Venue,
                    opt => opt.MapFrom(src => src.Venue))
                .ForMember(dest => dest.RegistrationStartTime,
                    opt => opt.MapFrom(src => src.RegistrationStartTime))
                .ForMember(dest => dest.RegistrationEndTime,
                    opt => opt.MapFrom(src => src.RegistrationEndTime))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForMember(dest => dest.LimitNumberOfPeople,
                    opt => opt.MapFrom(src => src.LimitNumberOfPeople))
                .ForMember(dest => dest.EnableVerify,
                    opt => opt.MapFrom(src => src.EnableVerify))
                .ForMember(dest => dest.EnableIdentityConfirmed,
                    opt => opt.MapFrom(src => src.EnableIdentityConfirmed))
                .ForPath(dest => dest.Files,
                    opt => opt.MapFrom(src => src.GeneralCampaignFiles))
                .ForPath(dest => dest.Company,
                    opt => opt.MapFrom(src => src.Company));

            #endregion
            
            #region EventEditDto 轉換成 Event

            CreateMap<GeneralCampaignEditDto, GeneralCampaign>()
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Content,
                    opt => opt.MapFrom(src => src.Content))
                .ForMember(dest => dest.Declaration,
                    opt => opt.MapFrom(src => src.Declaration))
                .ForMember(dest => dest.Venue,
                    opt => opt.MapFrom(src => src.Venue))
                .ForMember(dest => dest.RegistrationStartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.RegistrationEndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.StartTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.EndTime,
                    opt => opt.Ignore())
                .ForMember(dest => dest.LimitNumberOfPeople,
                    opt => opt.MapFrom(src => src.LimitNumberOfPeople))
                .ForMember(dest => dest.EnableVerify,
                    opt => opt.MapFrom(src => src.EnableVerify))
                .ForMember(dest => dest.EnableIdentityConfirmed,
                    opt => opt.MapFrom(src => src.EnableIdentityConfirmed))
                .AfterMap((src, dest) =>
                {
                    if (src.RegistrationStartDate != null && src.RegistrationStartTime != null)
                    {
                        var date = (DateTime) src.RegistrationStartDate;
                        var time = (DateTime) src.RegistrationStartTime;
                        dest.RegistrationStartTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                    }
                    else
                    {
                        dest.RegistrationStartTime = null;
                    }
                    if (src.RegistrationEndDate != null && src.RegistrationEndTime != null)
                    {
                        var date = (DateTime) src.RegistrationEndDate;
                        var time = (DateTime) src.RegistrationEndTime;
                        dest.RegistrationEndTime = new DateTime(date.Year, date.Month, date.Day, time.Hour, time.Minute, 0);
                    }
                    else
                    {
                        dest.RegistrationEndTime = null;
                    }
                    dest.StartTime = new DateTime(src.StartDate.Year, src.StartDate.Month, src.StartDate.Day, src.StartTime.Hour, src.StartTime.Minute, 0);
                    dest.EndTime = new DateTime(src.EndDate.Year, src.EndDate.Month, src.EndDate.Day, src.EndTime.Hour, src.EndTime.Minute, 0);
                });

            #endregion
            
            #region GeneralParticipantDto 轉換成 GeneralCampaignParticipant

            CreateMap<GeneralParticipantDto, GeneralCampaignParticipant>()
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ContactPhone,
                    opt => opt.MapFrom(src => src.ContactPhone))
                .ForMember(dest => dest.Remark,
                    opt => opt.MapFrom(src => src.Remark));

            #endregion
            
            #region GeneralCampaign 轉換成 MyGeneralCampaignDto

            CreateMap<GeneralCampaign, MyGeneralCampaignDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.GeneralCampaignId))
                .ForMember(dest => dest.Title,
                    opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.RegistrationStartTime,
                    opt => opt.MapFrom(src => src.RegistrationStartTime))
                .ForMember(dest => dest.RegistrationEndTime,
                    opt => opt.MapFrom(src => src.RegistrationEndTime))
                .ForMember(dest => dest.StartTime,
                    opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime,
                    opt => opt.MapFrom(src => src.EndTime))
                .ForPath(dest => dest.Status,
                    opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.Status = src.GeneralCampaignAttendees.First().Status;
                });

            #endregion
        }
    }
}