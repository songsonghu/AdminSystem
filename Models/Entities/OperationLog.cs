using AdminSystem.Common;

namespace AdminSystem.Models.Entities;

/// <summary>
/// 操作日志实体
/// </summary>
public class OperationLog : BaseEntity
{
    /// <summary>
    /// 操作用户 ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 操作用户名
    /// </summary>
    public string UserName { get; set; } = string.Empty;

    /// <summary>
    /// 操作类型
    /// </summary>
    public OperationType OperationType { get; set; }

    /// <summary>
    /// 操作模块
    /// </summary>
    public string Module { get; set; } = string.Empty;

    /// <summary>
    /// 操作内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 请求 URL
    /// </summary>
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求方法
    /// </summary>
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求参数
    /// </summary>
    public string? RequestParams { get; set; }

    /// <summary>
    /// 响应结果
    /// </summary>
    public string? Response { get; set; }

    /// <summary>
    /// 操作 IP
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// 操作地点
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
    /// 操作耗时（毫秒）
    /// </summary>
    public long Duration { get; set; }

    /// <summary>
    /// 是否成功
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? ErrorMessage { get; set; }
}
