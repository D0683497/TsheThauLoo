using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Activity;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class ActivityConfigurations
    {
        public static void ActivityRelation(ModelBuilder builder)
        {
            #region Event 跟 EventFile 一對多

            builder.Entity<EventFile>()
                .HasOne(file => file.Event)
                .WithMany(e => e.EventFiles)
                .HasForeignKey(file => file.EventId);

            #endregion
            
            #region Event 跟 EventAttendee 一對多

            builder.Entity<EventAttendee>()
                .HasOne(attendee => attendee.Event)
                .WithMany(e => e.EventAttendees)
                .HasForeignKey(attendee => attendee.EventId);

            #endregion
            
            #region ApplicationUser 跟 EventAttendee 一對多

            builder.Entity<EventAttendee>()
                .HasOne(attendee => attendee.ApplicationUser)
                .WithMany(user => user.EventAttendees)
                .HasForeignKey(attendee => attendee.ApplicationUserId);

            #endregion
            
            #region Event 跟 EventParticipant 一對多

            builder.Entity<EventParticipant>()
                .HasOne(participant => participant.Event)
                .WithMany(e => e.EventParticipants)
                .HasForeignKey(participant => participant.EventId);

            #endregion
            
            #region Campaign 跟 CampaignFile 一對多

            builder.Entity<CampaignFile>()
                .HasOne(file => file.Campaign)
                .WithMany(campaign => campaign.CampaignFiles)
                .HasForeignKey(file => file.CampaignId);

            #endregion
            
            #region Campaign 跟 GeneralCampaign 一對多

            builder.Entity<GeneralCampaign>()
                .HasOne(general => general.Campaign)
                .WithMany(campaign => campaign.GeneralCampaigns)
                .HasForeignKey(general => general.CampaignId);

            #endregion
            
            #region GeneralCampaign 跟 GeneralCampaignFile 一對多

            builder.Entity<GeneralCampaignFile>()
                .HasOne(file => file.GeneralCampaign)
                .WithMany(general => general.GeneralCampaignFiles)
                .HasForeignKey(file => file.GeneralCampaignId);

            #endregion
            
            #region GeneralCampaign 跟 GeneralCampaignAttendee 一對多

            builder.Entity<GeneralCampaignAttendee>()
                .HasOne(attendee => attendee.GeneralCampaign)
                .WithMany(general => general.GeneralCampaignAttendees)
                .HasForeignKey(attendee => attendee.GeneralCampaignId);

            #endregion
            
            #region ApplicationUser 跟 GeneralCampaignAttendee 一對多

            builder.Entity<GeneralCampaignAttendee>()
                .HasOne(attendee => attendee.ApplicationUser)
                .WithMany(user => user.GeneralCampaignAttendees)
                .HasForeignKey(attendee => attendee.ApplicationUserId);

            #endregion
            
            #region GeneralCampaign 跟 GeneralCampaignParticipant 一對多

            builder.Entity<GeneralCampaignParticipant>()
                .HasOne(participant => participant.GeneralCampaign)
                .WithMany(general => general.GeneralCampaignParticipants)
                .HasForeignKey(participant => participant.GeneralCampaignId);

            #endregion
            
            #region Campaign 跟 RecruitmentCampaign 一對多

            builder.Entity<RecruitmentCampaign>()
                .HasOne(recruitment => recruitment.Campaign)
                .WithMany(campaign => campaign.RecruitmentCampaigns)
                .HasForeignKey(recruitment => recruitment.CampaignId);

            #endregion
            
            #region RecruitmentCampaign 跟 RecruitmentCampaignFile 一對多

            builder.Entity<RecruitmentCampaignFile>()
                .HasOne(file => file.RecruitmentCampaign)
                .WithMany(recruitment => recruitment.RecruitmentCampaignFiles)
                .HasForeignKey(file => file.RecruitmentCampaignId);

            #endregion
        }
    }
}