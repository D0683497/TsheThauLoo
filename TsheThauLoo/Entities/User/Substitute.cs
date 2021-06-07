using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 職務代理人
    /// </summary>
    public class Substitute
    {
        /// <summary>
        /// 職務代理人識別碼
        /// </summary>
        [Key]
        public string SubstituteId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
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
        
        public string ManagerId { get; set; }

        public Manager Manager { get; set; }
    }
}