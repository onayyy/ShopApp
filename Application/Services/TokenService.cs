using Application.Authentication.Commands;
using Application.Common.Interfaces.RedisCache;
using Application.Common.Interfaces.Tokens;
using Application.DTOs;
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
        private readonly IRedisCacheService _redisCacheService;
        
        public TokenService(IOptions<TokenSettings> options, IRedisCacheService redisCacheService)
        {
            _tokenSettings = options.Value;
            _redisCacheService = redisCacheService;
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

            var tokenHandler = new JwtSecurityTokenHandler().WriteToken(token);

            var cacheKey = $"Token_{userLogin.Id}";
            var cacheExpiry = TimeSpan.FromMinutes(5);


            _redisCacheService.SetAsync(cacheKey, tokenHandler, DateTime.Now.Add(cacheExpiry));
            //_redisCacheService.SetAsync()

            return tokenHandler;
          
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
