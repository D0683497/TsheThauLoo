using System.Security.Claims;

namespace TsheThauLoo.Services;

public interface ITokenService
{
    /// <summary>
    /// 產生 JWT Token
    /// </summary>
    string GenerateJwtToken(IList<Claim> claims);
}