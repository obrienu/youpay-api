using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Youpay.API.Dtos;

namespace Youpay.API.Utils
{
    public class TokenUtil : ITokenUtil
    {
        private readonly IConfiguration _config;
        public TokenUtil(IConfiguration _config)
        {
            this._config = _config;

        }
        public string GenerateToken(TokenClaimsDto tokenClaimsDto)
        {
            Claim[] claims = null;

            if(tokenClaimsDto.Role != null){
                 claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenClaimsDto.Id.ToString()),
                    new Claim(ClaimTypes.Name, tokenClaimsDto.Email),
                    new Claim(ClaimTypes.Role , tokenClaimsDto.Role)
                };
            }
            else
            {
                  claims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, tokenClaimsDto.Id.ToString()),
                    new Claim(ClaimTypes.Name, tokenClaimsDto.Email)
                };
            }
                

            var key = new SymmetricSecurityKey(Encoding
                .UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

    }
}