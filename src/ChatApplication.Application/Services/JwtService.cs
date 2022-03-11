using ChatApplication.Application.Settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ChatApplication.Application.Services;

public interface IJwtService
{
    string GenerateJwtToken(int userId, string email);
}

public class JwtService : IJwtService
{
    private readonly JwtSetting _options;

    public JwtService(IOptions<JwtSetting> options)
    {
        _options = options.Value;
    }

    public string GenerateJwtToken(int userId, string email)
    {
        var expires = DateTime.UtcNow.AddDays(1);
        var expiry = expires.ToEpochTimeSpan().TotalSeconds.ToInt32();
        var claims = new List<Claim>
        {
            new(ClaimTypes.Email, email),
            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Expiration, expiry.ToString(CultureInfo.InvariantCulture))
        };
        var secretKey = Encoding.UTF8.GetBytes(_options.IssuerSigningKey);
        var signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256);

        var jwtToken = new JwtSecurityToken(_options.ValidIssuer, _options.ValidAudience, claims, expires: expires, signingCredentials: signingCredentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        return token;
    }
}