using System;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Job;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.Resume
{
    /// <summary>
    /// 徵才子活動履歷
    /// </summary>
    public class RecruitmentCampaignResume
    {
        /// <summary>
        /// 徵才子活動履歷識別碼
        /// </summary>
        [Key]
        public string RecruitmentCampaignResumeId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 履歷審查狀態
        /// </summary>
        [Required]
        public ResumeReviewType Type { get; set; }

        /// <summary>
        /// 面試機會
        /// </summary>
        [Required]
        public bool IsInterview { get; set; } = false;

        /// <summary>
        /// 錄取
        /// </summary>
        [Required]
        public bool IsHire { get; set; } = false;

        public string FileResumeId { get; set; }
        
        public FileResume FileResume { get; set; }

        public string ExaminerId { get; set; }
        
        public Examiner Examiner { get; set; }
        
        public string RecruitmentCampaignOpeningId { get; set; }

        public RecruitmentCampaignOpening RecruitmentCampaignOpening { get; set; }
    }
}