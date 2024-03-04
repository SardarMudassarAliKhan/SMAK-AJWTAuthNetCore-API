using SMAK_AJWTAuthNetCore_Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace SMAK_AJWTAuthNetCore_Services.Services
{
    // Service responsible for generating and validating JWT tokens
    public class TokenService : ITokenService
    {
        private readonly ITokenRepository _tokenRepository;

        // Constructor to initialize the TokenService with an instance of ITokenRepository
        public TokenService(ITokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository ?? throw new ArgumentNullException(nameof(tokenRepository));
        }

        // Method to generate an access token based on the provided claims
        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            // Validate input
            if (claims == null)
            {
                throw new ArgumentNullException(nameof(claims));
            }

            // Delegate token generation to the injected token repository
            return _tokenRepository.GenerateAccessToken(claims);
        }

        // Method to generate a refresh token
        public string GenerateRefreshToken()
        {
            // Delegate token generation to the injected token repository
            return _tokenRepository.GenerateRefreshToken();
        }

        // Method to extract ClaimsPrincipal from an expired token
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            // Validate input
            if (string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Token cannot be null or empty", nameof(token));
            }

            // Delegate token extraction to the injected token repository
            return _tokenRepository.GetPrincipalFromExpiredToken(token);
        }
    }
}
