using BoardApp.Models.Authorization;
using BoardWebApp.Definitions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BoardApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IConfiguration _configuration;

        public AuthController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration
        )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginQM model)
        {
            var accessToken = GenerateToken(await _userManager.FindByNameAsync(model.UserName));
            return Ok(new { accessToken });
        }

        private string GenerateToken(IdentityUser user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authorization:Jwt:SecretKey"]));

            var tokenIssueDateTime = DateTime.UtcNow;
            var tokenExpireDateTime = tokenIssueDateTime.Add(TimeSpan.FromDays(365));
            var claims = new List<Claim>()
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtCustomClaimNames.Id, user.Id)
                };

            var jwtToken = new JwtSecurityToken(
                //issuer: _configuration["Authorization:Jwt:Issuer"],
                //audience: _configuration["Authorization:Jwt:Audience"],
                claims: claims.ToArray(),
                //notBefore: tokenIssueDateTime,
                expires: tokenExpireDateTime,
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            var encodedJwt = jwtSecurityTokenHandler.WriteToken(jwtToken);

            return encodedJwt;
        }
    }
}
