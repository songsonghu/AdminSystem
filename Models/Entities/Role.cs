namespace AdminSystem.Models.Entities;

/// <summary>
/// 角色实体
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// 角色名称
    /// </summary>
    public string RoleName { get; set; } = string.Empty;

    /// <summary>
    /// 角色代码
    /// </summary>
    public string RoleCode { get; set; } = string.Empty;

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 用户角色关联
    /// </summary>
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

    /// <summary>
    /// 角色菜单关联
    /// </summary>
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}
