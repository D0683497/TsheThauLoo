using System;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Business;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 企業使用者
    /// </summary>
    public class Manager
    {
        /// <summary>
        /// 企業使用者識別碼
        /// </summary>
        [Key]
        public string ManagerId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 公司使用者驗證
        /// </summary>
        [Required]
        public bool ManagerConfirmed { get; set; } = false;
        
        /// <summary>
        /// 所屬部門/單位
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string DivisionName { get; set; }
        
        /// <summary>
        /// 職稱
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string JobTitle { get; set; }

        /// <summary>
        /// 聯絡用電子郵件
        /// </summary>
        [Required]
        [MaxLength(320)]
        public string ContactEmail { get; set; }

        /// <summary>
        /// 聯絡用電話號碼
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string ContactPhone { get; set; }

        /// <summary>
        /// 聯絡用地址
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string ContactAddress { get; set; }

        public Substitute Substitute { get; set; }
        
        public string CompanyId { get; set; }

        public Company Company { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}