using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User;

/// <summary>
/// 職務代理人
/// </summary>
[Owned]
public class Substitute
{
    /// <summary>
    /// 姓名
    /// </summary>
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Email { get; set; } = null!;

    /// <summary>
    /// 電話
    /// </summary>
    [Required]
    [MaxLength(30)]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// 地址
    /// </summary>
    [Required]
    [MaxLength(256)]
    public string Address { get; set; } = null!;

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
}