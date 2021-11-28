using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Resume;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 審查員
    /// </summary>
    public class Examiner
    {
        /// <summary>
        /// 審查員識別碼
        /// </summary>
        [Key]
        public string ExaminerId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 審查員驗證
        /// </summary>
        [Required]
        public bool ExaminerConfirmed { get; set; } = false;
        
        /// <summary>
        /// 工作單位
        /// </summary>
        [MaxLength(30)]
        public string DivisionName { get; set; }
        
        /// <summary>
        /// 職稱
        /// </summary>
        [MaxLength(30)]
        public string JobTitle { get; set; }

        public ICollection<RecruitmentCampaignResume> RecruitmentCampaignResumes { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}