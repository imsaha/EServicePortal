using System.Security.Claims;

namespace EServicePortal.Application.Common.Interfaces;

public interface ITokenService
{
    string GenerateToken(List<Claim> claims, DateTime? expires);
}
