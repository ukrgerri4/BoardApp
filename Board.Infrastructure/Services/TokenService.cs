using Board.Application.Interfaces.Services;
using Board.Domain.Definitions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Board.Infrastructure.Services
{
    public class TokenService: ITokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateToken(IdentityUser user)
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
