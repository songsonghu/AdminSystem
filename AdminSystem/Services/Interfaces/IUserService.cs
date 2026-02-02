using AdminSystem.Models.DTOs;
using AdminSystem.Models.ViewModels;

namespace AdminSystem.Services.Interfaces;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 用户登录
    /// </summary>
    Task<LoginResultDto?> LoginAsync(LoginDto loginDto);

    /// <summary>
    /// 根据 ID 获取用户
    /// </summary>
    Task<UserDto?> GetByIdAsync(int id);

    /// <summary>
    /// 分页查询用户
    /// </summary>
    Task<PagedResult<UserDto>> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null);

    /// <summary>
    /// 创建用户
    /// </summary>
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);

    /// <summary>
    /// 更新用户
    /// </summary>
    Task<bool> UpdateAsync(UpdateUserDto updateUserDto);

    /// <summary>
    /// 删除用户
    /// </summary>
    Task<bool> DeleteAsync(int id);

    /// <summary>
    /// 重置密码
    /// </summary>
    Task<bool> ResetPasswordAsync(int id);

    /// <summary>
    /// 修改密码
    /// </summary>
    Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto);

    /// <summary>
    /// 检查用户名是否存在
    /// </summary>
    Task<bool> IsUserNameExistsAsync(string userName, int? excludeUserId = null);
}
