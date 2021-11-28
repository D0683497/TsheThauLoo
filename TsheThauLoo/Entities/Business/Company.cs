using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Entities.Business
{
    /// <summary>
    /// 公司
    /// </summary>
    public class Company
    {
        /// <summary>
        /// 公司識別碼
        /// </summary>
        [Key]
        public string CompanyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 公司驗證
        /// </summary>
        [Required]
        public bool CompanyConfirmed { get; set; } = false;
        
        /// <summary>
        /// 統一編號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string RegistrationNumber { get; set; }
        
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        
        /// <summary>
        /// 簡介
        /// </summary>
        public string Introduction { get; set; }
        
        /// <summary>
        /// 網站
        /// </summary>
        [MaxLength(300)]
        public string Website { get; set; }

        public ICollection<IndustrialClassification> IndustrialClassifications { get; set; }
        
        public CompanyLogo CompanyLogo { get; set; }

        public CompanyVerify CompanyVerify { get; set; }

        public ICollection<Manager> Managers { get; set; }

        public ICollection<GeneralCampaign> GeneralCampaigns { get; set; }

        public ICollection<RecruitmentCampaign> RecruitmentCampaigns { get; set; }
    }
}