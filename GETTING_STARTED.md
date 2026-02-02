# AdminSystem - 快速开始指南

本文档介绍如何快速开始使用 AdminSystem 项目。

## 📋 项目说明

AdminSystem 是一个基于 ASP.NET Core 8.0 开发的通用后台管理系统，包含完整的用户管理、角色管理、菜单管理、部门管理等功能。

## 🛠️ 环境要求

- .NET 8.0 SDK
- SQL Server 2019+ 或 SQL Server LocalDB
- Visual Studio 2022 / VS Code / JetBrains Rider

## 🚀 快速开始

### 1. 克隆项目

```bash
git clone https://github.com/songsonghu/AdminSystem.git
cd AdminSystem
```

### 2. 还原依赖包

```bash
dotnet restore
```

### 3. 配置数据库

编辑 `appsettings.json`，修改数据库连接字符串（如需要）：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=AdminSystemDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

### 4. 初始化数据库

项目使用 EF Core 的 Code First 方式，首次运行时会自动创建数据库和种子数据。

如果需要手动创建迁移：

```bash
# 安装 EF Core 工具（如果尚未安装）
dotnet tool install --global dotnet-ef

# 创建迁移
dotnet ef migrations add InitialCreate

# 应用迁移
dotnet ef database update
```

### 5. 运行项目

```bash
dotnet run
```

或者使用热重载模式：

```bash
dotnet watch run
```

项目启动后，浏览器会自动打开 Swagger 页面：
- HTTPS: https://localhost:5001/swagger
- HTTP: http://localhost:5000/swagger

## 🔑 默认账号

系统初始化后会自动创建默认管理员账号：

- **用户名**: `admin`
- **密码**: `123456`

⚠️ **重要提示**：首次登录后请及时修改默认密码！

## 📚 API 使用说明

### 1. 用户登录

**接口地址**: `POST /api/account/login`

**请求参数**:
```json
{
  "userName": "admin",
  "password": "123456"
}
```

**响应示例**:
```json
{
  "success": true,
  "message": "登录成功",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "user": {
      "id": 1,
      "userName": "admin",
      "realName": "系统管理员",
      "status": 0,
      "roles": ["SuperAdmin"]
    }
  }
}
```

### 2. 获取用户信息

**接口地址**: `GET /api/account/info`

**请求头**:
```
Authorization: Bearer {token}
```

### 3. 用户管理 API

所有用户管理接口都需要在请求头中携带 JWT Token。

- `GET /api/user/{id}` - 获取用户详情
- `GET /api/user/page` - 分页查询用户
- `POST /api/user` - 创建用户
- `PUT /api/user` - 更新用户
- `DELETE /api/user/{id}` - 删除用户
- `POST /api/user/reset-password/{id}` - 重置密码
- `POST /api/user/change-password` - 修改密码

详细的 API 文档请访问 Swagger 页面。

## 🗂️ 项目结构

```
AdminSystem/
├── Common/              # 公共类（枚举、常量）
├── Controllers/         # API 控制器
├── Data/               # 数据访问层
│   ├── Repositories/   # 仓储模式实现
│   └── ApplicationDbContext.cs
├── Helpers/            # 工具类（MD5、JWT）
├── Models/             # 数据模型
│   ├── Entities/       # 实体类
│   ├── DTOs/           # 数据传输对象
│   └── ViewModels/     # 视图模型
├── Services/           # 业务服务层
│   ├── Interfaces/     # 服务接口
│   └── Implementations/# 服务实现
└── Program.cs          # 程序启动入口
```

## ✨ 核心功能

1. **用户管理**
   - 用户的增删改查
   - 用户状态管理
   - 密码管理（重置、修改）

2. **认证授权**
   - JWT Token 认证
   - 基于角色的权限控制

3. **数据持久化**
   - Entity Framework Core
   - 仓储模式
   - 软删除

4. **统一返回格式**
   - 成功/失败状态
   - 统一的错误处理
   - 分页查询支持

## 🔒 安全特性

- 密码 MD5 加密存储
- JWT Token 认证
- 软删除保护数据
- 防止删除系统管理员
- CORS 配置支持跨域

## 📖 开发指南

### 添加新实体

1. 在 `Models/Entities/` 中创建实体类（继承 `BaseEntity`）
2. 在 `ApplicationDbContext` 中添加 `DbSet`
3. 配置实体关系和约束
4. 创建迁移并更新数据库

### 添加新服务

1. 在 `Services/Interfaces/` 中定义接口
2. 在 `Services/Implementations/` 中实现接口
3. 在 `Program.cs` 中注册服务

### 添加新控制器

1. 在 `Controllers/` 中创建控制器
2. 继承 `ControllerBase`
3. 使用 `[ApiController]` 和 `[Route]` 特性
4. 需要认证的接口添加 `[Authorize]` 特性

## 🐛 常见问题

### Q1: 数据库连接失败

**A**: 检查 SQL Server 服务是否启动，或者将连接字符串改为你本地的数据库实例。

### Q2: 端口被占用

**A**: 修改 `Properties/launchSettings.json` 中的端口配置。

### Q3: JWT Token 过期

**A**: Token 默认 2 小时过期，可在 `Common/Constants.cs` 中修改 `ExpirationMinutes` 值。

## 📞 技术支持

如有问题或建议，欢迎提交 Issue 或 Pull Request。

- GitHub: https://github.com/songsonghu/AdminSystem
- Issues: https://github.com/songsonghu/AdminSystem/issues

## 📄 许可证

本项目采用 MIT 许可证。详见 [LICENSE](LICENSE) 文件。
