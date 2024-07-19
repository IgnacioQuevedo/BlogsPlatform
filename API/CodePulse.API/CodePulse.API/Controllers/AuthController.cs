using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Host;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITokenRepository _tokenRepository;

        public AuthController(UserManager<IdentityUser> userManager,
            ITokenRepository tokenRepository)
        {
            _userManager = userManager;
            _tokenRepository = tokenRepository;
        }
        
        //POST: {apibaseurl}/api/auth/login
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO userToLogin)
        {
            
            // Check email
           var identityUser = await _userManager.FindByEmailAsync(userToLogin.Email.Trim());

           if (identityUser != null)
           {
               //Check Password
               var passwordChecked = await _userManager.CheckPasswordAsync(identityUser, userToLogin.Password);
               if (passwordChecked)
               {
                   var userRoles = await _userManager.GetRolesAsync(identityUser);
                   //Create a token and return response
                   var jwtToken = _tokenRepository.createJwtToken(identityUser, userRoles.ToList());
                   var response = new LoginResponseDTO()
                   {
                       Email = userToLogin.Email,
                       Roles = userRoles.ToList(),
                       Token = jwtToken
                   };
                   return Ok(response);
               }
           }
           ModelState.AddModelError("","Email or Password Incorrect");

           return ValidationProblem(ModelState);
           
        }
        
        //POST: {apibaseurl}/api/auth/register
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO userToRegister)
        {
            // CREAR EL USUARIO (IdentityUser)
            var user = new IdentityUser
            {
                UserName = userToRegister.email?.Trim(),
                Email = userToRegister.email?.Trim()
            };

            var identityResult = await _userManager.CreateAsync(user, userToRegister.password);

            if (identityResult.Succeeded)
            {
                // Add Role to User (Reader Role)
                identityResult = await _userManager.AddToRoleAsync(user, "Reader");

                if (identityResult.Succeeded)
                {
                    return Ok();
                }
                
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            else
            {
                if (identityResult.Errors.Any())
                {
                    foreach (var error in identityResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }

            return ValidationProblem(ModelState);
        }
    }
}