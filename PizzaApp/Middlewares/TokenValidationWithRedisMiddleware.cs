using Application.Common.Interfaces.RedisCache;
using Application.Common.Interfaces.Tokens;
using Application.DTOs;
using Domain.Model;
using Infrastructure.Tokens;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopAPI.Middlewares
{
    public class TokenValidationWithRedisMiddleware
    {
         private readonly RequestDelegate _next;
         private readonly IRedisCacheService _redisCacheService;

         public TokenValidationWithRedisMiddleware(RequestDelegate next, IRedisCacheService redisCacheService)
         {
             _next = next;
             _redisCacheService = redisCacheService;
         }

         public async Task InvokeAsync(HttpContext context)
         {
             var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

             if (token != null)
             {
                 var tokenHandler = new JwtSecurityTokenHandler();
                 JwtSecurityToken jwtToken;

                 try
                 {
                     jwtToken = tokenHandler.ReadJwtToken(token);
                 }
                 catch
                 {
                     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     await context.Response.WriteAsync("Invalid token.");
                     return;
                 }

                 var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;
                 var cacheKey = $"Token_{userId}";
                 var cachedToken = await _redisCacheService.GetAsync<string>(cacheKey);

                 if (cachedToken == null || cachedToken != token)
                 {
                     context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     await context.Response.WriteAsync("Token expired or not found. Please log in again.");
                     return;
                 }
             }

             await _next(context);
         }


    }
}

