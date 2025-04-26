namespace HeadLessBlog.Application.Common.Interfaces;
public interface ITokenService
{
    string GenerateToken(Guid userId, string role);
}