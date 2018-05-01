using System;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace SignalRChatDemo.Controllers
{
    public class TokenController : Controller
    {
        [HttpPost("api/token")]
        public string GetToken([FromQuery] string userId)
        {
            return GenerateToken(userId);
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

}