using AdminSystem.Models.DTOs;
using AdminSystem.Models.ViewModels;
using AdminSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AdminSystem.Controllers;

/// <summary>
/// 用户管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户详情
    /// </summary>
    /// <param name="id">用户 ID</param>
    /// <returns>用户信息</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResult<UserDto>>> GetById(int id)
    {
        try
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return Ok(ApiResult<UserDto>.Fail("用户不存在"));
            }

            return Ok(ApiResult<UserDto>.Ok(user));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户详情失败，用户 ID: {Id}", id);
            return Ok(ApiResult<UserDto>.Fail("获取用户详情失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 分页查询用户
    /// </summary>
    /// <param name="pageIndex">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="keyword">关键字</param>
    /// <returns>用户列表</returns>
    [HttpGet("page")]
    public async Task<ActionResult<ApiResult<PagedResult<UserDto>>>> GetPaged(
        [FromQuery] int pageIndex = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? keyword = null)
    {
        try
        {
            var result = await _userService.GetPagedAsync(pageIndex, pageSize, keyword);
            return Ok(ApiResult<PagedResult<UserDto>>.Ok(result));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "分页查询用户失败");
            return Ok(ApiResult<PagedResult<UserDto>>.Fail("分页查询用户失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 创建用户
    /// </summary>
    /// <param name="createUserDto">用户信息</param>
    /// <returns>创建的用户</returns>
    [HttpPost]
    public async Task<ActionResult<ApiResult<UserDto>>> Create([FromBody] CreateUserDto createUserDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createUserDto.UserName))
            {
                return Ok(ApiResult<UserDto>.Fail("用户名不能为空"));
            }

            if (string.IsNullOrWhiteSpace(createUserDto.Password))
            {
                return Ok(ApiResult<UserDto>.Fail("密码不能为空"));
            }

            var user = await _userService.CreateAsync(createUserDto);
            _logger.LogInformation("创建用户成功，用户名: {UserName}", createUserDto.UserName);
            return Ok(ApiResult<UserDto>.Ok(user, "创建用户成功"));
        }
        catch (InvalidOperationException ex)
        {
            return Ok(ApiResult<UserDto>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建用户失败");
            return Ok(ApiResult<UserDto>.Fail("创建用户失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 更新用户
    /// </summary>
    /// <param name="updateUserDto">用户信息</param>
    /// <returns>操作结果</returns>
    [HttpPut]
    public async Task<ActionResult<ApiResult<bool>>> Update([FromBody] UpdateUserDto updateUserDto)
    {
        try
        {
            var result = await _userService.UpdateAsync(updateUserDto);
            if (!result)
            {
                return Ok(ApiResult<bool>.Fail("用户不存在"));
            }

            _logger.LogInformation("更新用户成功，用户 ID: {Id}", updateUserDto.Id);
            return Ok(ApiResult<bool>.Ok(true, "更新用户成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新用户失败，用户 ID: {Id}", updateUserDto.Id);
            return Ok(ApiResult<bool>.Fail("更新用户失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    /// <param name="id">用户 ID</param>
    /// <returns>操作结果</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult<ApiResult<bool>>> Delete(int id)
    {
        try
        {
            var result = await _userService.DeleteAsync(id);
            if (!result)
            {
                return Ok(ApiResult<bool>.Fail("用户不存在"));
            }

            _logger.LogInformation("删除用户成功，用户 ID: {Id}", id);
            return Ok(ApiResult<bool>.Ok(true, "删除用户成功"));
        }
        catch (InvalidOperationException ex)
        {
            return Ok(ApiResult<bool>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "删除用户失败，用户 ID: {Id}", id);
            return Ok(ApiResult<bool>.Fail("删除用户失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="id">用户 ID</param>
    /// <returns>操作结果</returns>
    [HttpPost("reset-password/{id}")]
    public async Task<ActionResult<ApiResult<bool>>> ResetPassword(int id)
    {
        try
        {
            var result = await _userService.ResetPasswordAsync(id);
            if (!result)
            {
                return Ok(ApiResult<bool>.Fail("用户不存在"));
            }

            _logger.LogInformation("重置密码成功，用户 ID: {Id}", id);
            return Ok(ApiResult<bool>.Ok(true, "重置密码成功，新密码为：123456"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "重置密码失败，用户 ID: {Id}", id);
            return Ok(ApiResult<bool>.Fail("重置密码失败：" + ex.Message));
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    /// <param name="changePasswordDto">修改密码信息</param>
    /// <returns>操作结果</returns>
    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResult<bool>>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        try
        {
            var userIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdStr) || !int.TryParse(userIdStr, out var userId))
            {
                return Ok(ApiResult<bool>.Fail("无法获取用户信息"));
            }

            var result = await _userService.ChangePasswordAsync(userId, changePasswordDto);
            if (!result)
            {
                return Ok(ApiResult<bool>.Fail("用户不存在"));
            }

            _logger.LogInformation("修改密码成功，用户 ID: {UserId}", userId);
            return Ok(ApiResult<bool>.Ok(true, "修改密码成功"));
        }
        catch (InvalidOperationException ex)
        {
            return Ok(ApiResult<bool>.Fail(ex.Message));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "修改密码失败");
            return Ok(ApiResult<bool>.Fail("修改密码失败：" + ex.Message));
        }
    }
}
