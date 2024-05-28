using Application.Authentication.Commands;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Interfaces.Tokens
{
    public interface ITokenService
    {
        string CreateToken(UserAggregate userLogin);

        string GenerateRefreshToken();

        ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token);
    }
}
