using AdminSystem.Models.DTOs;
using AdminSystem.Models.ViewModels;
using AdminSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminSystem.Controllers;

/// <summary>
/// 账户控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<AccountController> _logger;

    public AccountController(IUserService userService, ILogger<AccountController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="loginDto">登录信息</param>
    /// <returns>登录结果</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResult<LoginResultDto>>> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(loginDto.UserName) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return Ok(ApiResult<LoginResultDto>.Fail("用户名和密码不能为空"));
            }

            var result = await _userService.LoginAsync(loginDto);
            if (result == null)
            {
                return Ok(ApiResult<LoginResultDto>.Fail("用户名或密码错误"));
            }

            _logger.LogInformation("用户 {UserName} 登录成功", loginDto.UserName);
            return Ok(ApiResult<LoginResultDto>.Ok(result, "登录成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登录失败");
            return Ok(ApiResult<LoginResultDto>.Fail("登录失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 用户登出
    /// </summary>
    /// <returns>操作结果</returns>
    [HttpPost("logout")]
    [Authorize]
    public async Task<ActionResult<ApiResult<object?>>> Logout()
    {
        try
        {
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            _logger.LogInformation("用户 {UserName} 登出", userName);
            await Task.CompletedTask;
            return Ok(ApiResult<object?>.Ok(null, "登出成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登出失败");
            return Ok(ApiResult<object?>.Fail("登出失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 获取当前用户信息
    /// </summary>
    /// <returns>用户信息</returns>
    [HttpGet("info")]
    [Authorize]
    public async Task<ActionResult<ApiResult<UserDto>>> GetUserInfo()
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Ok(ApiResult<UserDto>.Fail("无法获取用户信息"));
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return Ok(ApiResult<UserDto>.Fail("用户不存在"));
            }

            return Ok(ApiResult<UserDto>.Ok(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户信息失败");
            return Ok(ApiResult<UserDto>.Fail("获取用户信息失败：" + ex.Message));
        }
    }
}
