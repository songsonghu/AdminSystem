namespace AdminSystem.Models.Entities;

/// <summary>
/// 登录日志实体
/// </summary>
public class LoginLog : BaseEntity
{
    /// <summary>
    /// 用户 ID
    /// </summary>
    public int? UserId { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 登录 IP
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 登录地点
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 浏览器
    /// </summary>
    public string? Browser { get; set; }

    /// <summary>
    /// 操作系统
    /// </summary>
    public string? Os { get; set; }

    /// <summary>
    /// 登录时间
    /// </summary>
    public DateTime LoginTime { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 失败原因
    /// </summary>
    public string? FailureReason { get; set; }

    /// <summary>
    /// 登录类型（账号密码、短信、扫码等）
    /// </summary>
    public string? LoginType { get; set; }
}
