namespace AdminSystem.Common;

/// <summary>
/// 用户状态枚举
/// </summary>
public enum UserStatus
{
    /// <summary>
    /// 正常
    /// </summary>
    Normal = 0,

    /// <summary>
    /// 禁用
    /// </summary>
    Disabled = 1
}

/// <summary>
/// 操作类型枚举
/// </summary>
public enum OperationType
{
    /// <summary>
    /// 查询
    /// </summary>
    Query = 0,

    /// <summary>
    /// 新增
    /// </summary>
    Create = 1,

    /// <summary>
    /// 更新
    /// </summary>
    Update = 2,

    /// <summary>
    /// 删除
    /// </summary>
    Delete = 3,

    /// <summary>
    /// 登录
    /// </summary>
    Login = 4,

    /// <summary>
    /// 登出
    /// </summary>
    Logout = 5,

    /// <summary>
    /// 导出
    /// </summary>
    Export = 6,

    /// <summary>
    /// 导入
    /// </summary>
    Import = 7
}

/// <summary>
/// 菜单类型枚举
/// </summary>
public enum MenuType
{
    /// <summary>
    /// 目录
    /// </summary>
    Catalog = 0,

    /// <summary>
    /// 菜单
    /// </summary>
    Menu = 1,

    /// <summary>
    /// 按钮
    /// </summary>
    Button = 2
}
