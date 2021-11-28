using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 校友
    /// </summary>
    public class Alumnus
    {
        /// <summary>
        /// 校友識別碼
        /// </summary>
        [Key]
        public string AlumnusId { get; set; } =  Guid.NewGuid().ToString();
        
        /// <summary>
        /// 校友驗證
        /// </summary>
        [Required]
        public bool AlumnusConfirmed { get; set; } = false;

        /// <summary>
        /// 畢業年度
        /// </summary>
        [MaxLength(10)]
        public string DateOfGraduation { get; set; }

        /// <summary>
        /// 畢業學院
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string College { get; set; }

        /// <summary>
        /// 畢業系所
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Department { get; set; }
        
        /// <summary>
        /// 畢業班級
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Class { get; set; }

        public AlumnusVerify AlumnusVerify { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}