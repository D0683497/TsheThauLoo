using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 負責業務
    /// </summary>
    public class Responsibility
    {
        /// <summary>
        /// 負責業務識別碼
        /// </summary>
        [Key]
        public string ResponsibilityId { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Description  { get; set; }
        
        public string AdministratorId { get; set; }
        
        public Administrator Administrator { get; set; }
    }
}