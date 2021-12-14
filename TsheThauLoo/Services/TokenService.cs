using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TsheThauLoo.Entities.Identity;
using TsheThauLoo.Utils;

namespace TsheThauLoo.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateJwtToken(IList<Claim> claims)
    {
        claims.Add(new (JwtClaims.JWTID, Nanoid.Nanoid.Generate(size: 25)));
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
        // HmacSha256Signature 要求 key 至少要 16 字元以上
        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

        var header = new JwtHeader(signingCredentials);
        var payload = new JwtPayload(
            issuer: _configuration["JwtSettings:Issuer"],
            audience: _configuration["JwtSettings:Audience"],
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.AddMinutes(5),
            issuedAt: DateTime.UtcNow);
        var securityToken  = new JwtSecurityToken(header, payload);
        var serializeToken = new JwtSecurityTokenHandler().WriteToken(securityToken);
        return serializeToken;
    }
}