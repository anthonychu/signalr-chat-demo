using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SignalRChatDemo.Data;

namespace SignalRChatDemo.Controllers
{
    public class TokenController : Controller
    {
        private readonly SignInManager<ApplicationUser> signInManager;

        public TokenController(SignInManager<ApplicationUser> signInManager)
        {
            this.signInManager = signInManager;
        }

        [HttpPost("api/token")]
        public async Task<IActionResult> GetTokenAsync([FromBody] LoginRequest login)
        {
            var result = await signInManager.PasswordSignInAsync(login.Username, login.Password, false, true);
            return result.Succeeded ? (IActionResult)Ok(GenerateToken(login.Username)) : Unauthorized();
        }

        private string GenerateToken(string userId)
        {
            var key = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(Constants.DefaultKey));
            
            var claims = new[]
            { 
                new Claim(ClaimTypes.NameIdentifier, userId)
            };
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken("SignalRTestServer", "SignalRTests", claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: credentials);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}