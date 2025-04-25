using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HeadLessBlog.WebAPI.Services;

public interface ITokenService
{
    string GenerateToken(Guid userId, string role);
}

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly bool _isDevelopment;

    public TokenService(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _configuration = configuration;
        _isDevelopment = environment.IsDevelopment();
    }

    public string GenerateToken(Guid userId, string role)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("SecretKey not configured.");

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(ClaimTypes.Role, role),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Unique token ID
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(6),
            SigningCredentials = credentials
        };

        if (!_isDevelopment)
        {
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            tokenDescriptor.Issuer = issuer;
            tokenDescriptor.Audience = audience;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}
