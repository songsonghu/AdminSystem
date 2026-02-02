namespace AdminSystem.Models.Entities;

/// <summary>
/// 用户角色关联实体
/// </summary>
public class UserRole
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 用户（导航属性）
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// 角色 ID
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 角色（导航属性）
    /// </summary>
    public Role Role { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
