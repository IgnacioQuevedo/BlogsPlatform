using System.Drawing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.Host;
using Microsoft.IdentityModel.Tokens;

namespace CodePulse.API.Repositories.Implementation;

public class TokenRepository : ITokenRepository
{
    private readonly IConfiguration _configuration;

    public TokenRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string createJwtToken(IdentityUser user, List<string> roles)
    {
        
        // Create Claims
        // A claim is a piece of information about a user that is relevant to an application.
        // Claims represent attributes associated with the user, such as their name, email, role,
        // or any other custom information. These claims are typically represented as key-value pairs.
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, user.Email)
        };
        
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        // JWT Security Token Parameters

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        //Return the token

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}