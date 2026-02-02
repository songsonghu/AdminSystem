using AdminSystem.Common;

namespace AdminSystem.Models.Entities;

/// <summary>
/// 菜单实体
/// </summary>
public class Menu : BaseEntity
{
    /// <summary>
    /// 菜单名称
    /// </summary>
    public string MenuName { get; set; } = string.Empty;

    /// <summary>
    /// 菜单代码
    /// </summary>
    public string MenuCode { get; set; } = string.Empty;

    /// <summary>
    /// 菜单类型
    /// </summary>
    public MenuType MenuType { get; set; }

    /// <summary>
    /// 父级菜单 ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 父级菜单（导航属性）
    /// </summary>
    public Menu? Parent { get; set; }

    /// <summary>
    /// 子菜单列表
    /// </summary>
    public ICollection<Menu> Children { get; set; } = new List<Menu>();

    /// <summary>
    /// 路由路径
    /// </summary>
    public string? Path { get; set; }

    /// <summary>
    /// 组件路径
    /// </summary>
    public string? Component { get; set; }

    /// <summary>
    /// 图标
    /// </summary>
    public string? Icon { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否可见
    /// </summary>
    public bool IsVisible { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    public string? Remark { get; set; }

    /// <summary>
    /// 角色菜单关联
    /// </summary>
    public ICollection<RoleMenu> RoleMenus { get; set; } = new List<RoleMenu>();
}
