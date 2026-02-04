namespace AdminSystem.Common;

/// <summary>
/// 系统常量
/// </summary>
public static class Constants
{
    /// <summary>
    /// 默认密码（123456）
    /// </summary>
    public const string DefaultPassword = "123456";

    /// <summary>
    /// 默认密码的 MD5 值（E10ADC3949BA59ABBE56E057F20F883E）
    /// </summary>
    public const string DefaultPasswordMd5 = "E10ADC3949BA59ABBE56E057F20F883E";

    /// <summary>
    /// JWT 配置
    /// </summary>
    public static class Jwt
    {
        /// <summary>
        /// JWT 密钥
        /// </summary>
        public const string Secret = "AdminSystem_JWT_Secret_Key_2024_Very_Long_Secret_Key_For_Security";

        /// <summary>
        /// JWT 安全密钥 (别名)
        /// </summary>
        public const string SecurityKey = Secret;

        /// <summary>
        /// 发行者
        /// </summary>
        public const string Issuer = "AdminSystem";

        /// <summary>
        /// 受众
        /// </summary>
        public const string Audience = "AdminSystemAPI";

        /// <summary>
        /// 过期时间（分钟）
        /// </summary>
        public const int ExpirationMinutes = 120;
    }

    /// <summary>
    /// 缓存键
    /// </summary>
    public static class CacheKeys
    {
        /// <summary>
        /// 用户信息缓存键前缀
        /// </summary>
        public const string UserInfoPrefix = "UserInfo_";

        /// <summary>
        /// 用户权限缓存键前缀
        /// </summary>
        public const string UserPermissionPrefix = "UserPermission_";

        /// <summary>
        /// 菜单树缓存键
        /// </summary>
        public const string MenuTree = "MenuTree";
    }

    /// <summary>
    /// 系统角色
    /// </summary>
    public static class SystemRoles
    {
        /// <summary>
        /// 超级管理员
        /// </summary>
        public const string SuperAdmin = "SuperAdmin";

        /// <summary>
        /// 管理员
        /// </summary>
        public const string Admin = "Admin";

        /// <summary>
        /// 普通用户
        /// </summary>
        public const string User = "User";
    }

    /// <summary>
    /// 分页配置
    /// </summary>
    public static class Pagination
    {
        /// <summary>
        /// 默认页码
        /// </summary>
        public const int DefaultPageIndex = 1;

        /// <summary>
        /// 默认每页数量
        /// </summary>
        public const int DefaultPageSize = 10;

        /// <summary>
        /// 最大每页数量
        /// </summary>
        public const int MaxPageSize = 100;
    }
}
