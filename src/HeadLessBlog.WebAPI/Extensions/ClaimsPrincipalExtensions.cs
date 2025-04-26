using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace HeadLessBlog.WebAPI.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user)
    {
        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier) ?? user.FindFirst(JwtRegisteredClaimNames.Sub);

        if (userIdClaim is null)
        {
            throw new InvalidOperationException("UserId claim not found.");
        }

        return Guid.Parse(userIdClaim.Value);
    }
}
