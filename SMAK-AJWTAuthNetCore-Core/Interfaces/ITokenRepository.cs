using System.Security.Claims;

namespace SMAK_AJWTAuthNetCore_Core.Interfaces
{
    public interface ITokenRepository
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
