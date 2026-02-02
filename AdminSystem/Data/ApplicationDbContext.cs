using AdminSystem.Common;
using AdminSystem.Helpers;
using AdminSystem.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace AdminSystem.Data;

/// <summary>
/// 数据库上下文
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // DbSet 定义
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<Menu> Menus { get; set; } = null!;
    public DbSet<Department> Departments { get; set; } = null!;
    public DbSet<UserRole> UserRoles { get; set; } = null!;
    public DbSet<RoleMenu> RoleMenus { get; set; } = null!;
    public DbSet<OperationLog> OperationLogs { get; set; } = null!;
    public DbSet<LoginLog> LoginLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置软删除全局查询过滤器
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(r => !r.IsDeleted);
        modelBuilder.Entity<Menu>().HasQueryFilter(m => !m.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(d => !d.IsDeleted);
        modelBuilder.Entity<OperationLog>().HasQueryFilter(o => !o.IsDeleted);
        modelBuilder.Entity<LoginLog>().HasQueryFilter(l => !l.IsDeleted);

        // 配置 User 实体
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.UserName).IsRequired().HasMaxLength(50);
            entity.Property(u => u.Password).IsRequired().HasMaxLength(100);
            entity.Property(u => u.RealName).HasMaxLength(50);
            entity.Property(u => u.Phone).HasMaxLength(20);
            entity.Property(u => u.Email).HasMaxLength(100);
            entity.Property(u => u.Avatar).HasMaxLength(500);
            entity.Property(u => u.LastLoginIp).HasMaxLength(50);

            // 唯一索引
            entity.HasIndex(u => u.UserName).IsUnique();

            // 与 Department 的关系
            entity.HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置 Role 实体
        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.Property(r => r.RoleName).IsRequired().HasMaxLength(50);
            entity.Property(r => r.RoleCode).IsRequired().HasMaxLength(50);
            entity.Property(r => r.Description).HasMaxLength(500);

            // 唯一索引
            entity.HasIndex(r => r.RoleCode).IsUnique();
        });

        // 配置 Menu 实体
        modelBuilder.Entity<Menu>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.Property(m => m.MenuName).IsRequired().HasMaxLength(50);
            entity.Property(m => m.MenuCode).IsRequired().HasMaxLength(50);
            entity.Property(m => m.Path).HasMaxLength(200);
            entity.Property(m => m.Component).HasMaxLength(200);
            entity.Property(m => m.Icon).HasMaxLength(50);
            entity.Property(m => m.Remark).HasMaxLength(500);

            // 唯一索引
            entity.HasIndex(m => m.MenuCode).IsUnique();

            // 自关联关系
            entity.HasOne(m => m.Parent)
                .WithMany(m => m.Children)
                .HasForeignKey(m => m.ParentId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 配置 Department 实体
        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.DepartmentName).IsRequired().HasMaxLength(50);
            entity.Property(d => d.DepartmentCode).IsRequired().HasMaxLength(50);
            entity.Property(d => d.Phone).HasMaxLength(20);
            entity.Property(d => d.Email).HasMaxLength(100);
            entity.Property(d => d.Description).HasMaxLength(500);

            // 唯一索引
            entity.HasIndex(d => d.DepartmentCode).IsUnique();

            // 自关联关系
            entity.HasOne(d => d.Parent)
                .WithMany(d => d.Children)
                .HasForeignKey(d => d.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // 与 Manager 的关系
            entity.HasOne(d => d.Manager)
                .WithMany()
                .HasForeignKey(d => d.ManagerId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // 配置 UserRole 实体（多对多关系）
        modelBuilder.Entity<UserRole>(entity =>
        {
            entity.HasKey(ur => new { ur.UserId, ur.RoleId });

            entity.HasOne(ur => ur.User)
                .WithMany(u => u.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置 RoleMenu 实体（多对多关系）
        modelBuilder.Entity<RoleMenu>(entity =>
        {
            entity.HasKey(rm => new { rm.RoleId, rm.MenuId });

            entity.HasOne(rm => rm.Role)
                .WithMany(r => r.RoleMenus)
                .HasForeignKey(rm => rm.RoleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(rm => rm.Menu)
                .WithMany(m => m.RoleMenus)
                .HasForeignKey(rm => rm.MenuId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置 OperationLog 实体
        modelBuilder.Entity<OperationLog>(entity =>
        {
            entity.HasKey(o => o.Id);
            entity.Property(o => o.UserName).IsRequired().HasMaxLength(50);
            entity.Property(o => o.Module).IsRequired().HasMaxLength(50);
            entity.Property(o => o.Content).HasMaxLength(500);
            entity.Property(o => o.RequestUrl).HasMaxLength(500);
            entity.Property(o => o.RequestMethod).HasMaxLength(10);
            entity.Property(o => o.IpAddress).HasMaxLength(50);
            entity.Property(o => o.Location).HasMaxLength(100);
            entity.Property(o => o.Browser).HasMaxLength(100);
            entity.Property(o => o.Os).HasMaxLength(100);

            entity.HasIndex(o => o.UserId);
            entity.HasIndex(o => o.CreatedAt);
        });

        // 配置 LoginLog 实体
        modelBuilder.Entity<LoginLog>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.Property(l => l.UserName).IsRequired().HasMaxLength(50);
            entity.Property(l => l.IpAddress).HasMaxLength(50);
            entity.Property(l => l.Location).HasMaxLength(100);
            entity.Property(l => l.Browser).HasMaxLength(100);
            entity.Property(l => l.Os).HasMaxLength(100);
            entity.Property(l => l.FailureReason).HasMaxLength(500);
            entity.Property(l => l.LoginType).HasMaxLength(20);

            entity.HasIndex(l => l.UserId);
            entity.HasIndex(l => l.LoginTime);
        });

        // 种子数据
        SeedData(modelBuilder);
    }

    /// <summary>
    /// 种子数据
    /// </summary>
    private void SeedData(ModelBuilder modelBuilder)
    {
        // 1. 添加默认角色
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                Id = 1,
                RoleName = "超级管理员",
                RoleCode = Constants.SystemRoles.SuperAdmin,
                Description = "系统超级管理员，拥有所有权限",
                Sort = 1,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Role
            {
                Id = 2,
                RoleName = "管理员",
                RoleCode = Constants.SystemRoles.Admin,
                Description = "系统管理员",
                Sort = 2,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            new Role
            {
                Id = 3,
                RoleName = "普通用户",
                RoleCode = Constants.SystemRoles.User,
                Description = "普通用户",
                Sort = 3,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );

        // 2. 添加默认部门
        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                Id = 1,
                DepartmentName = "总公司",
                DepartmentCode = "HQ",
                ParentId = null,
                Sort = 1,
                IsEnabled = true,
                Description = "总公司",
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );

        // 3. 添加默认管理员用户（admin/123456）
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                UserName = "admin",
                Password = Constants.DefaultPasswordMd5,
                RealName = "系统管理员",
                Email = "admin@admin.com",
                Status = UserStatus.Normal,
                DepartmentId = 1,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );

        // 4. 分配管理员角色
        modelBuilder.Entity<UserRole>().HasData(
            new UserRole
            {
                UserId = 1,
                RoleId = 1,
                CreatedAt = DateTime.UtcNow
            }
        );

        // 5. 添加默认菜单
        modelBuilder.Entity<Menu>().HasData(
            // 系统管理（目录）
            new Menu
            {
                Id = 1,
                MenuName = "系统管理",
                MenuCode = "System",
                MenuType = MenuType.Catalog,
                ParentId = null,
                Path = "/system",
                Icon = "setting",
                Sort = 1,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 用户管理（菜单）
            new Menu
            {
                Id = 2,
                MenuName = "用户管理",
                MenuCode = "System:User",
                MenuType = MenuType.Menu,
                ParentId = 1,
                Path = "/system/user",
                Component = "system/user/index",
                Icon = "user",
                Sort = 1,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 角色管理（菜单）
            new Menu
            {
                Id = 3,
                MenuName = "角色管理",
                MenuCode = "System:Role",
                MenuType = MenuType.Menu,
                ParentId = 1,
                Path = "/system/role",
                Component = "system/role/index",
                Icon = "team",
                Sort = 2,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 菜单管理（菜单）
            new Menu
            {
                Id = 4,
                MenuName = "菜单管理",
                MenuCode = "System:Menu",
                MenuType = MenuType.Menu,
                ParentId = 1,
                Path = "/system/menu",
                Component = "system/menu/index",
                Icon = "menu",
                Sort = 3,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 部门管理（菜单）
            new Menu
            {
                Id = 5,
                MenuName = "部门管理",
                MenuCode = "System:Department",
                MenuType = MenuType.Menu,
                ParentId = 1,
                Path = "/system/department",
                Component = "system/department/index",
                Icon = "apartment",
                Sort = 4,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 日志管理（目录）
            new Menu
            {
                Id = 6,
                MenuName = "日志管理",
                MenuCode = "Log",
                MenuType = MenuType.Catalog,
                ParentId = null,
                Path = "/log",
                Icon = "file-text",
                Sort = 2,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 操作日志（菜单）
            new Menu
            {
                Id = 7,
                MenuName = "操作日志",
                MenuCode = "Log:Operation",
                MenuType = MenuType.Menu,
                ParentId = 6,
                Path = "/log/operation",
                Component = "log/operation/index",
                Icon = "file-text",
                Sort = 1,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            },
            // 登录日志（菜单）
            new Menu
            {
                Id = 8,
                MenuName = "登录日志",
                MenuCode = "Log:Login",
                MenuType = MenuType.Menu,
                ParentId = 6,
                Path = "/log/login",
                Component = "log/login/index",
                Icon = "login",
                Sort = 2,
                IsVisible = true,
                IsEnabled = true,
                CreatedAt = DateTime.UtcNow,
                IsDeleted = false
            }
        );

        // 6. 分配超级管理员所有菜单权限
        modelBuilder.Entity<RoleMenu>().HasData(
            new RoleMenu { RoleId = 1, MenuId = 1, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 2, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 3, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 4, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 5, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 6, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 7, CreatedAt = DateTime.UtcNow },
            new RoleMenu { RoleId = 1, MenuId = 8, CreatedAt = DateTime.UtcNow }
        );
    }
}
