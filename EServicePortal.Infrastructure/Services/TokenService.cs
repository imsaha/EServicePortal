using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Ardalis.GuardClauses;
using EServicePortal.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace EServicePortal.Infrastructure.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(List<Claim> claims, DateTime? expires)
    {
        var key = Guard.Against.NullOrWhiteSpace(_configuration["APP_SECRET"]);
        var symmetricKey = Convert.FromBase64String(key);
        var tokenHandler = new JwtSecurityTokenHandler();
        var identity = new ClaimsIdentity(claims);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = identity,
            Expires = expires,
            IssuedAt = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(symmetricKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }
}
