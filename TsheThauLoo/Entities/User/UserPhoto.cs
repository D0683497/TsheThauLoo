using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 使用者照片
    /// </summary>
    public class UserPhoto : Document
    {
        /// <summary>
        /// 使用者照片識別碼
        /// </summary>
        [Key]
        public string UserPhotoId { get; set; } = Guid.NewGuid().ToString();
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}