using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 管理員
    /// </summary>
    public class Administrator
    {
        /// <summary>
        /// 管理員識別碼
        /// </summary>
        [Key]
        public string AdministratorId { get; set; } =  Guid.NewGuid().ToString();
        
        /// <summary>
        /// 管理員驗證
        /// </summary>
        [Required]
        public bool AdministratorConfirmed { get; set; } = false;

        /// <summary>
        /// 顯示於關於頁面
        /// </summary>
        [Required]
        public bool ShowAbout { get; set; } = false;
        
        /// <summary>
        /// 證號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string NetworkId { get; set; }
        
        /// <summary>
        /// 部門
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Dept { get; set; }

        /// <summary>
        /// 單位
        /// </summary>
        [MaxLength(20)]
        public string Unit { get; set; }
        
        /// <summary>
        /// 職稱
        /// </summary>
        [MaxLength(20)]
        public string JobTitle { get; set; }

        /// <summary>
        /// 分機
        /// </summary>
        [MaxLength(10)]
        public string Extension { get; set; }

        /// <summary>
        /// 聯絡用電子郵件
        /// </summary>
        [MaxLength(320)]
        public string ContactEmail { get; set; }

        public ICollection<Responsibility> Responsibilities { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}