# AdminSystem - ASP.NET Core MVC 通用管理系统

## 📋 项目简介

这是一个基于 ASP.NET Core MVC 开发的通用后台管理系统，采用单项目架构，适合中小型企业快速开发和部署。

## 🚀 技术栈

- **框架**：ASP.NET Core 8.0 MVC
- **ORM**：Entity Framework Core 8.0
- **数据库**：SQL Server (支持 LocalDB)
- **认证**：JWT (JSON Web Token)
- **日志**：Serilog
- **架构**：Repository + Service 模式

## ✨ 核心功能

- ✅ 用户管理（增删改查、密码管理）
- ✅ 角色管理（角色权限配置）
- ✅ 菜单管理（树形菜单、按钮权限）
- ✅ 部门管理（组织架构树）
- ✅ 操作日志（用户行为追踪）
- ✅ 登录日志（登录历史记录）
- ✅ JWT 认证授权
- ✅ 统一返回格式
- ✅ 分页查询
- ✅ 软删除

## 📁 项目结构

```
AdminSystem/
├── Controllers/          # 控制器
├── Models/              # 数据模型
│   ├── Entities/       # 实体类
│   ├── DTOs/           # 数据传输对象
│   └── ViewModels/     # 视图模型
├── Services/            # 业务服务
│   ├── Interfaces/     # 服务接口
│   └── Implementations/# 服务实现
├── Data/                # 数据访问
│   ├── Repositories/   # 仓储
│   └── Migrations/     # 数据库迁移
├── Helpers/             # 工具类
├── Common/              # 公共类
├── wwwroot/            # 静态文件
└── appsettings.json    # 配置文件
```

## 🎯 快速开始

### 1. 克隆项目

```bash
git clone https://github.com/songsonghu/AdminSystem.git
cd AdminSystem
```

### 2. 安装依赖

```bash
dotnet restore
```

### 3. 配置数据库

项目默认使用 SQL Server LocalDB，连接字符串在 `appsettings.json` 中配置：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AdminSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

如果需要使用其他数据库，请修改连接字符串。

### 4. 执行数据库迁移

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 5. 运行项目

```bash
dotnet run
```

项目默认运行在：
- HTTPS: https://localhost:5001
- HTTP: http://localhost:5000

### 6. 访问 Swagger

在浏览器中打开：https://localhost:5001/swagger

## 🔑 默认账号

系统初始化后会自动创建管理员账号：

- **用户名**：admin
- **密码**：123456

⚠️ 首次登录后请及时修改密码！

## 📚 API 文档

### 账户管理

| 接口 | 方法 | 说明 |
|------|------|------|
| /api/account/login | POST | 用户登录 |
| /api/account/logout | POST | 用户退出 |
| /api/account/info | GET | 获取当前用户信息 |

### 用户管理

| 接口 | 方法 | 说明 |
|------|------|------|
| /api/user/{id} | GET | 获取用户详情 |
| /api/user/page | GET | 分页���询用户 |
| /api/user | POST | 创建用户 |
| /api/user | PUT | 更新用户 |
| /api/user/{id} | DELETE | 删除用户 |
| /api/user/reset-password/{id} | POST | 重置密码 |
| /api/user/change-password | POST | 修改密码 |

## 🛠️ 开发工具

推荐使用以下 IDE：
- Visual Studio 2022
- Visual Studio Code + C# 扩展
- JetBrains Rider

## 📦 依赖包

主要 NuGet 包：

```xml
<PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Tools" Version="8.0.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
<PackageReference Include="AutoMapper.Extensions.Microsoft.DependencyInjection" Version="12.0.1" />
<PackageReference Include="Serilog.AspNetCore" Version="8.0.0" />
<PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />
```

## 📝 数据库设计

主要数据表：

- **Users** - 用户表
- **Roles** - 角色表
- **Menus** - 菜单表
- **Departments** - 部门表
- **UserRoles** - 用户角色关联表
- **RoleMenus** - 角色菜单关联表
- **OperationLogs** - 操作日志表
- **LoginLogs** - 登录日志表

## 🔐 安全说明

1. 密码使用 MD5 加密存储（生产环境建议使用 BCrypt 或 PBKDF2）
2. JWT Token 过期时间默认 2 小时
3. 支持软删除，数据不会真正删除
4. 所有 API 需要 JWT 认证（除登录接口外）

## 🤝 贡献

欢迎提交 Issue 和 Pull Request！

## 📄 许可证

本项目采用 [MIT](LICENSE) 许可证。

## 👨‍💻 作者

- GitHub: [@songsonghu](https://github.com/songsonghu)

## 📧 联系方式

如有问题或建议，欢迎通过以下方式联系：

- 提交 GitHub Issue
- 发送邮件至项目维护者

---

⭐ 如果这个项目对您有帮助，欢迎 Star 支持！