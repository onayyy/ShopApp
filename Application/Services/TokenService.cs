using Application.Authentication.Commands;
using Application.Common.Interfaces.RedisCache;
using Application.Common.Interfaces.Tokens;
using Domain.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly TokenSettings _tokenSettings;
        
        public TokenService(IOptions<TokenSettings> options)
        {
            _tokenSettings = options.Value;
        }

        public string CreateToken(UserAggregate userLogin)
        {
            if (_tokenSettings.Key is null)
                throw new Exception("Key değeri boş olamaz.");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, userLogin.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, userLogin.Email),
            };

            var token = new JwtSecurityToken(_tokenSettings.Issuer, _tokenSettings.Audience, claims, expires: DateTime.Now.AddMinutes(10), signingCredentials: credentials);

            /*var refreshToken = GenerateRefreshToken();
            _ = int.TryParse("_tokenSettings.RefreshTokenValidityInDays", out int refreshTokenValidityInDays);

            userLogin*/

            return new JwtSecurityTokenHandler().WriteToken(token);
          
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        public ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Key)),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Geçersiz Token.");

            return principal;
        }
    }
}
