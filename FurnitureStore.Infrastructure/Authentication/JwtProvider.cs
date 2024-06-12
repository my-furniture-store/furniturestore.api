using FurnitureStore.Application.Common.Interfaces;
using FurnitureStore.Domain.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FurnitureStore.Infrastructure.Authentication;

public class JwtProvider : IJwtProvider
{
    private readonly JwtOptions _jwtOptions;

    public JwtProvider(IOptions<JwtOptions> jwtOptions)
    {
        _jwtOptions = jwtOptions.Value;
    }
    public string GenerateUserAccessToken(User user)
    {
        var claims = new Claim[] 
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString(), ClaimValueTypes.Integer64)
        };

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            notBefore: null,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: signingCredentials
            );

        return new JwtSecurityTokenHandler().WriteToken(token);   
    }

    public DateTime? GetTokenExpiryDate(string token)
    {
        if(string.IsNullOrWhiteSpace(token)) 
            return null;

        var jwtToken = new JwtSecurityTokenHandler().ReadToken(token.Replace("\"", string.Empty)) as JwtSecurityToken;

        var expiryDate = jwtToken?.ValidTo;

        return expiryDate;
    }
}
