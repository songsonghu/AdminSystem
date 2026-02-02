using AdminSystem.Common;
using AdminSystem.Data;
using AdminSystem.Data.Repositories;
using AdminSystem.Helpers;
using AdminSystem.Models.DTOs;
using AdminSystem.Models.Entities;
using AdminSystem.Models.ViewModels;
using AdminSystem.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AdminSystem.Services.Implementations;

/// <summary>
/// 用户服务实现
/// </summary>
public class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly ApplicationDbContext _context;

    public UserService(
        IRepository<User> userRepository,
        IRepository<UserRole> userRoleRepository,
        ApplicationDbContext context)
    {
        _userRepository = userRepository;
        _userRoleRepository = userRoleRepository;
        _context = context;
    }

    /// <inheritdoc />
    public async Task<LoginResultDto?> LoginAsync(LoginDto loginDto)
    {
        // 查找用户
        var user = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.UserName == loginDto.UserName);

        if (user == null)
        {
            return null;
        }

        // 验证密码
        var passwordMd5 = MD5Helper.Encrypt(loginDto.Password);
        if (user.Password != passwordMd5)
        {
            return null;
        }

        // 检查用户状态
        if (user.Status != UserStatus.Normal)
        {
            return null;
        }

        // 获取角色列表
        var roles = user.UserRoles
            .Where(ur => ur.Role.IsEnabled)
            .Select(ur => ur.Role.RoleCode)
            .ToList();

        // 生成 Token
        var token = JwtHelper.GenerateToken(user.Id, user.UserName, roles);

        // 更新最后登录信息
        user.LastLoginTime = DateTime.UtcNow;
        await _userRepository.UpdateAsync(user);

        // 返回登录结果
        var userDto = MapToUserDto(user);
        return new LoginResultDto
        {
            Token = token,
            User = userDto
        };
    }

    /// <inheritdoc />
    public async Task<UserDto?> GetByIdAsync(int id)
    {
        var user = await _context.Users
            .Include(u => u.Department)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.Id == id);

        return user == null ? null : MapToUserDto(user);
    }

    /// <inheritdoc />
    public async Task<PagedResult<UserDto>> GetPagedAsync(int pageIndex, int pageSize, string? keyword = null)
    {
        var query = _context.Users
            .Include(u => u.Department)
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsQueryable();

        // 关键字搜索
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(u =>
                u.UserName.Contains(keyword) ||
                (u.RealName != null && u.RealName.Contains(keyword)) ||
                (u.Phone != null && u.Phone.Contains(keyword)) ||
                (u.Email != null && u.Email.Contains(keyword))
            );
        }

        var totalCount = await query.CountAsync();
        var users = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var userDtos = users.Select(MapToUserDto).ToList();

        return new PagedResult<UserDto>
        {
            Items = userDtos,
            TotalCount = totalCount,
            PageIndex = pageIndex,
            PageSize = pageSize
        };
    }

    /// <inheritdoc />
    public async Task<UserDto> CreateAsync(CreateUserDto createUserDto)
    {
        // 检查用户名是否存在
        if (await IsUserNameExistsAsync(createUserDto.UserName))
        {
            throw new InvalidOperationException("用户名已存在");
        }

        // 创建用户
        var user = new User
        {
            UserName = createUserDto.UserName,
            Password = MD5Helper.Encrypt(createUserDto.Password),
            RealName = createUserDto.RealName,
            Phone = createUserDto.Phone,
            Email = createUserDto.Email,
            Status = createUserDto.Status,
            DepartmentId = createUserDto.DepartmentId,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user);

        // 分配角色
        if (createUserDto.RoleIds.Any())
        {
            var userRoles = createUserDto.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.UserRoles.AddRangeAsync(userRoles);
            await _context.SaveChangesAsync();
        }

        return (await GetByIdAsync(user.Id))!;
    }

    /// <inheritdoc />
    public async Task<bool> UpdateAsync(UpdateUserDto updateUserDto)
    {
        var user = await _userRepository.GetByIdAsync(updateUserDto.Id);
        if (user == null)
        {
            return false;
        }

        // 更新用户信息
        user.RealName = updateUserDto.RealName;
        user.Phone = updateUserDto.Phone;
        user.Email = updateUserDto.Email;
        user.Status = updateUserDto.Status;
        user.DepartmentId = updateUserDto.DepartmentId;

        await _userRepository.UpdateAsync(user);

        // 更新角色
        var existingUserRoles = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .ToListAsync();

        _context.UserRoles.RemoveRange(existingUserRoles);

        if (updateUserDto.RoleIds.Any())
        {
            var newUserRoles = updateUserDto.RoleIds.Select(roleId => new UserRole
            {
                UserId = user.Id,
                RoleId = roleId,
                CreatedAt = DateTime.UtcNow
            }).ToList();

            await _context.UserRoles.AddRangeAsync(newUserRoles);
        }

        await _context.SaveChangesAsync();
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> DeleteAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        // 不能删除 admin 用户
        if (user.UserName == "admin")
        {
            throw new InvalidOperationException("不能删除系统管理员");
        }

        await _userRepository.DeleteAsync(user);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ResetPasswordAsync(int id)
    {
        var user = await _userRepository.GetByIdAsync(id);
        if (user == null)
        {
            return false;
        }

        user.Password = Constants.DefaultPasswordMd5;
        await _userRepository.UpdateAsync(user);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> ChangePasswordAsync(int userId, ChangePasswordDto changePasswordDto)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user == null)
        {
            return false;
        }

        // 验证旧密码
        var oldPasswordMd5 = MD5Helper.Encrypt(changePasswordDto.OldPassword);
        if (user.Password != oldPasswordMd5)
        {
            throw new InvalidOperationException("旧密码不正确");
        }

        // 验证新密码和确认密码是否一致
        if (changePasswordDto.NewPassword != changePasswordDto.ConfirmPassword)
        {
            throw new InvalidOperationException("新密码和确认密码不一致");
        }

        // 更新密码
        user.Password = MD5Helper.Encrypt(changePasswordDto.NewPassword);
        await _userRepository.UpdateAsync(user);
        return true;
    }

    /// <inheritdoc />
    public async Task<bool> IsUserNameExistsAsync(string userName, int? excludeUserId = null)
    {
        var query = _context.Users.Where(u => u.UserName == userName);
        if (excludeUserId.HasValue)
        {
            query = query.Where(u => u.Id != excludeUserId.Value);
        }
        return await query.AnyAsync();
    }

    /// <summary>
    /// 映射到 UserDto
    /// </summary>
    private UserDto MapToUserDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            UserName = user.UserName,
            RealName = user.RealName,
            Phone = user.Phone,
            Email = user.Email,
            Avatar = user.Avatar,
            Status = user.Status,
            DepartmentId = user.DepartmentId,
            DepartmentName = user.Department?.DepartmentName,
            Roles = user.UserRoles
                .Where(ur => ur.Role.IsEnabled)
                .Select(ur => ur.Role.RoleCode)
                .ToList(),
            LastLoginTime = user.LastLoginTime,
            LastLoginIp = user.LastLoginIp,
            CreatedAt = user.CreatedAt
        };
    }
}
