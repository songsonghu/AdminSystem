using System.Text;
using AdminSystem.Common;
using AdminSystem.Data;
using AdminSystem.Data.Repositories;
using AdminSystem.Services.Implementations;
using AdminSystem.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// 添加服务到容器

// 1. 配置数据库上下文
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// 2. 配置仓储和服务
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserService, UserService>();

// 3. 配置 JWT 认证
var jwtSecret = Constants.Jwt.Secret;
var key = Encoding.UTF8.GetBytes(jwtSecret);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = true,
        ValidIssuer = Constants.Jwt.Issuer,
        ValidateAudience = true,
        ValidAudience = Constants.Jwt.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero
    };
});

// 4. 配置授权
builder.Services.AddAuthorization();

// 5. 配置 CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 6. 配置控制器
builder.Services.AddControllers();

// 7. 配置 Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "AdminSystem API",
        Version = "v1",
        Description = "ASP.NET Core MVC 通用管理系统 API",
        Contact = new OpenApiContact
        {
            Name = "AdminSystem",
            Url = new Uri("https://github.com/songsonghu/AdminSystem")
        }
    });

    // 添加 JWT 认证支持
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// 配置 HTTP 请求管道

// 1. 开发环境配置
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "AdminSystem API v1");
        c.RoutePrefix = "swagger";
    });
}

// 2. HTTPS 重定向
app.UseHttpsRedirection();

// 3. 静态文件
app.UseStaticFiles();

// 4. 路由
app.UseRouting();

// 5. CORS
app.UseCors("AllowAll");

// 6. 认证和授权
app.UseAuthentication();
app.UseAuthorization();

// 7. 映射控制器
app.MapControllers();

// 8. 默认路由
app.MapGet("/", () => Results.Redirect("/swagger"));

// 9. 自动创建数据库和应用迁移
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        // 确保数据库已创建并应用所有迁移
        context.Database.EnsureCreated();
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("数据库已初始化");
        logger.LogInformation("默认管理员账号: admin");
        logger.LogInformation("默认密码: 123456");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "数据库初始化失败");
    }
}

app.Run();
