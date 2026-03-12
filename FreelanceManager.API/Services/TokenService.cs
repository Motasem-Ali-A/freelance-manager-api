using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FreelanceManager.Core.interfaces;
using FreelanceManager.Core.Models;
using Microsoft.IdentityModel.Tokens;

namespace FreelanceManager.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    public string CreateToken(AppUser user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
            new Claim(ClaimTypes.Name, user.FullName)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtData:Secret"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiry = DateTime.UtcNow.AddDays(Convert.ToDouble(_configuration["JwtData:ExpiryDays"]));

        var token = new JwtSecurityToken(
            issuer: _configuration["JwtData:Issuer"],
            audience: _configuration["JwtData:Audience"],
            claims: claims,
            expires: expiry,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}