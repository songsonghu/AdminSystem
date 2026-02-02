namespace AdminSystem.Models.Entities;

/// <summary>
/// 角色菜单关联实体
/// </summary>
public class RoleMenu
{
    /// <summary>
    /// 角色 ID
    /// </summary>
    public int RoleId { get; set; }

    /// <summary>
    /// 角色（导航属性）
    /// </summary>
    public Role Role { get; set; } = null!;

    /// <summary>
    /// 菜单 ID
    /// </summary>
    public int MenuId { get; set; }

    /// <summary>
    /// 菜单（导航属性）
    /// </summary>
    public Menu Menu { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
