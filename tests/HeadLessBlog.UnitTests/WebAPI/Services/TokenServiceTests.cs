using FluentAssertions;
using HeadLessBlog.WebAPI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using Moq;
using System.IdentityModel.Tokens.Jwt;

namespace HeadLessBlog.UnitTests.WebAPI.Services;

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

        var mockEnvironment = new Mock<IWebHostEnvironment>();
        mockEnvironment.Setup(env => env.EnvironmentName).Returns("Development");

        _tokenService = new TokenService(configuration, mockEnvironment.Object);
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

        jwtToken.Claims.Should().Contain(c => c.Type == "sub" && c.Value == userId.ToString());
        jwtToken.Claims.Should().Contain(c => c.Type == "role" && c.Value == role);
        jwtToken.ValidTo.Should().BeAfter(DateTime.UtcNow);
    }
}
