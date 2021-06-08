using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.Job
{
    /// <summary>
    /// 徵才子活動職缺
    /// </summary>
    public class RecruitmentCampaignOpening
    {
        /// <summary>
        /// 徵才子活動職缺識別碼
        /// </summary>
        [Key]
        public string RecruitmentCampaignOpeningId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 職缺單位/部門
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string DivisionName { get; set; }
        
        /// <summary>
        /// 職務名稱
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string JobTitle { get; set; }
        
        /// <summary>
        /// 工作內容
        /// </summary>
        [Required]
        public string JobDescription { get; set; }
        
        /// <summary>
        /// 工作地點
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string WorkPlace { get; set; }
        
        /// <summary>
        /// 薪資
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Salary { get; set; }

        /// <summary>
        /// 需求人數
        /// </summary>
        [Required]
        public int RequiredNumber { get; set; } = 0;

        /// <summary>
        /// 學歷
        /// </summary>
        [Required]
        public EducationType Education { get; set; }

        /// <summary>
        /// 相關工作經驗
        /// </summary>
        [MaxLength(500)]
        public string WorkExperience { get; set; }

        /// <summary>
        /// 語言能力
        /// </summary>
        [MaxLength(100)]
        public string Language { get; set; }

        /// <summary>
        /// 聘用人員國籍
        /// </summary>
        [MaxLength(20)]
        public string Nationality { get; set; }

        /// <summary>
        /// 身心障礙者應徵
        /// </summary>
        [Required]
        public bool IsAccessibility { get; set; } = false;
        
        public ICollection<Qualification> Qualifications { get; set; }
        
        public ICollection<Faculty> Faculties { get; set; }
        
        public ICollection<RecruitmentCampaignResume> RecruitmentCampaignResumes { get; set; }
        
        public string RecruitmentCampaignId { get; set; }

        public RecruitmentCampaign RecruitmentCampaign { get; set; }
    }
}