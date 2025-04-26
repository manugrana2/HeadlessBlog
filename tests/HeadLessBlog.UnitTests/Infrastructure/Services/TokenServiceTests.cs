using FluentAssertions;
using HeadLessBlog.Application.Common.Interfaces;
using HeadLessBlog.Infrastructure.Security;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;

namespace HeadLessBlog.UnitTests.Infrastructure.Services;

public class TokenServiceTests
{
    private readonly ITokenService _tokenService;

    public TokenServiceTests()
    {
        var inMemorySettings = new Dictionary<string, string?>
        {
            { "JwtSettings:SecretKey", "SuperSecretKeyForJwtTokenOfMinimum32Chars!" },
            { "JwtSettings:Issuer", "TestIssuer" },
            { "JwtSettings:Audience", "TestAudience" }
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings!)
            .Build();

        _tokenService = new TokenService(configuration);
    }

    [Fact]
    public void GenerateToken_ShouldReturn_ValidJwtToken_WithUserIdAndRoleClaims()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var role = "Creator";

        // Act
        var token = _tokenService.GenerateToken(userId, role);

        // Assert
        token.Should().NotBeNullOrWhiteSpace();

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        jwtToken.Claims.Should().Contain(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == userId.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == role);
        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
