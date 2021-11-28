using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Business
{
    /// <summary>
    /// 公司標識
    /// </summary>
    public class CompanyLogo : Document
    {
        /// <summary>
        /// 公司標識識別碼
        /// </summary>
        [Key]
        public string CompanyLogoId { get; set; } = Guid.NewGuid().ToString();
        
        public string CompanyId { get; set; }

        public Company Company { get; set; }
    }
}