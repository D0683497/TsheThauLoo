using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Entities.User;

/// <summary>
/// 企業使用者
/// </summary>
public class Employee
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [Key]
    [MaxLength(25)]
    public string Id { get; set; } = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);

    /// <summary>
    /// 職稱
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 部門
    /// </summary>
    [Required]
    [MaxLength(20)]
    public string Division { get; set; } = null!;

    /// <summary>
    /// 職務代理人
    /// </summary>
    public Substitute Substitute { get; set; } = null!;

    [Required]
    [MaxLength(25)]
    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual ApplicationUser User { get; set; } = null!;
}