using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AdminSystem.Common;
using Microsoft.IdentityModel.Tokens;

namespace AdminSystem.Helpers;

/// <summary>
/// JWT 帮助类
/// </summary>
public static class JwtHelper
{
    /// <summary>
    /// 生成 JWT Token
    /// </summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="userName">用户名</param>
    /// <param name="roles">角色列表</param>
    /// <returns>JWT Token</returns>
    public static string GenerateToken(int userId, string userName, List<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, userName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        // 添加角色声明
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Jwt.Secret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: Constants.Jwt.Issuer,
            audience: Constants.Jwt.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Constants.Jwt.ExpirationMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    /// <summary>
    /// 验证 JWT Token
    /// </summary>
    /// <param name="token">JWT Token</param>
    /// <returns>是否有效</returns>
    public static bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(Constants.Jwt.Secret);

        try
        {
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = true,
                ValidIssuer = Constants.Jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = Constants.Jwt.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            }, out _);

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// 从 Token 中获取用户 ID
    /// </summary>
    /// <param name="token">JWT Token</param>
    /// <returns>用户 ID</returns>
    public static int? GetUserIdFromToken(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadJwtToken(token);
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }
}
