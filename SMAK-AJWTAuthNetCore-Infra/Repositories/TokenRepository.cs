using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SMAK_AJWTAuthNetCore_Core.Entities;
using SMAK_AJWTAuthNetCore_Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SMAK_AJWTAuthNetCore_Infra.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        private readonly JsonWebTokenKeys JsonWebTokenKeys;

        public TokenRepository(JsonWebTokenKeys jsonWebTokenKeys)
        {
            this.JsonWebTokenKeys = jsonWebTokenKeys;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonWebTokenKeys.securityKey));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
            var tokeOptions = new JwtSecurityToken(
                issuer: JsonWebTokenKeys.ValidIssuer,
                audience: JsonWebTokenKeys.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: signinCredentials
            );
            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }
        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = JsonWebTokenKeys.ValidateAudience, // you might want to validate the audience and issuer depending on your use case
                ValidateIssuer = JsonWebTokenKeys.ValidateIssuer,
                ValidateIssuerSigningKey = true, // assuming this should always be true for security reasons
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JsonWebTokenKeys.IssuerSigningKey)),
                ValidateLifetime = JsonWebTokenKeys.ValidateLifetime
                //here we are saying that we don't care about the token's expiration date
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }
    }
}
