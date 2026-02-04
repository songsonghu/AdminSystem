using AdminSystem.Common;
using System.ComponentModel.DataAnnotations;

namespace AdminSystem.Models.DTOs;

/// <summary>
/// 用户 DTO
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 头像 URL
    /// </summary>
    public string? Avatar { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// 部门 ID
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// 部门名称
    /// </summary>
    public string? DepartmentName { get; set; }

    /// <summary>
    /// 角色列表
    /// </summary>
    public List<string> Roles { get; set; } = new();

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录 IP
    /// </summary>
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 创建用户 DTO
/// </summary>
public class CreateUserDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; } = UserStatus.Normal;

    /// <summary>
    /// 部门 ID
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// 角色 ID 列表
    /// </summary>
    public List<int> RoleIds { get; set; } = new();
}

/// <summary>
/// 更新用户 DTO
/// </summary>
public class UpdateUserDto
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string? RealName { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string? Phone { get; set; }

    /// <summary>
    /// 邮箱
    /// </summary>
    public string? Email { get; set; }

    /// <summary>
    /// 用户状态
    /// </summary>
    public UserStatus Status { get; set; }

    /// <summary>
    /// 部门 ID
    /// </summary>
    public int? DepartmentId { get; set; }

    /// <summary>
    /// 角色 ID 列表
    /// </summary>
    public List<int> RoleIds { get; set; } = new();
}

/// <summary>
/// 登录 DTO
/// </summary>
public class LoginDto
{
    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 密码
    /// </summary>
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 登录结果 DTO
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// JWT Token
    /// </summary>
    public string Token { get; set; } = string.Empty;

    /// <summary>
    /// 用户信息
    /// </summary>
    public UserDto User { get; set; } = null!;
}

/// <summary>
/// 修改密码 DTO
/// </summary>
public class ChangePasswordDto
{
    /// <summary>
    /// 旧密码
    /// </summary>
    public string OldPassword { get; set; } = string.Empty;

    /// <summary>
    /// 新密码
    /// </summary>
    public string NewPassword { get; set; } = string.Empty;

    /// <summary>
    /// 确认新密码
    /// </summary>
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 重置密码 DTO
/// </summary>
public class ResetPasswordDto
{
    /// <summary>
    /// 新密码
    /// </summary>
    [Required(ErrorMessage = "新密码不能为空")]
    [MinLength(6, ErrorMessage = "密码长度不能少于6位")]
    public string NewPassword { get; set; } = string.Empty;
}
