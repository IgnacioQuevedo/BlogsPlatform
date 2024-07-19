using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using Microsoft.AspNetCore.Identity;

namespace CodePulse.API.Repositories.Interface;

public interface ITokenRepository
{
    string createJwtToken(IdentityUser user,List<string> roles);
    

}
