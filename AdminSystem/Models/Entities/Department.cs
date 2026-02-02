namespace AdminSystem.Models.Entities;

/// <summary>
/// 部门实体
/// </summary>
public class Department : BaseEntity
{
    /// <summary>
    /// 部门名称
    /// </summary>
    public string DepartmentName { get; set; } = string.Empty;

    /// <summary>
    /// 部门代码
    /// </summary>
    public string DepartmentCode { get; set; } = string.Empty;

    /// <summary>
    /// 父级部门 ID
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 父级部门（导航属性）
    /// </summary>
    public Department? Parent { get; set; }

    /// <summary>
    /// 子部门列表
    /// </summary>
    public ICollection<Department> Children { get; set; } = new List<Department>();

    /// <summary>
    /// 负责人 ID
    /// </summary>
    public int? ManagerId { get; set; }

    /// <summary>
    /// 负责人（导航属性）
    /// </summary>
    public User? Manager { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int Sort { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// 部门员工
    /// </summary>
    public ICollection<User> Users { get; set; } = new List<User>();
}
