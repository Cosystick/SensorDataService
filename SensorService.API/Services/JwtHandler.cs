using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SensorService.API.Models;

namespace SensorService.Api.Services
{

    public interface IJwtHandler
    {
        JsonWebToken Create(User user);
    }


    public class JwtHandler : IJwtHandler
    {
        private readonly IConfiguration _configuration;
        //private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        //private readonly JwtHeader _jwtHeader;
        //private readonly SigningCredentials _signingCredentials;

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            //SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            //_signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            //_jwtHeader = new JwtHeader(_signingCredentials);
        }

        public JsonWebToken Create(User user)
        {
            //var nowUtc = DateTime.UtcNow;
            //var expiryMinutes = int.Parse(_configuration["Jwt:ExpiryMinutes"]);
            //var expires = nowUtc.AddMinutes(expiryMinutes);
            //var centuryBegin = new DateTime(1970, 1, 1).ToUniversalTime();
            //var exp = (long)(new TimeSpan(expires.Ticks - centuryBegin.Ticks).TotalSeconds);
            //var iat = (long)(new TimeSpan(nowUtc.Ticks - centuryBegin.Ticks).TotalSeconds);
            //var payload = new JwtPayload
            //{
            //    {"sub", user.UserName},
            //    {"iss", _configuration["Jwt:Issuer"]},
            //    {"iat", iat},
            //    {"exp", exp},
            //    {"UserId", user.Id },
            //    {"unique_name", user.UserName},
            //};

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim("UserId", user.Id.ToString())
            //};

            //if (user.IsAdministrator)
            //{
            //    var adminClaim = new Claim(ClaimTypes.Role, "Administrator");
            //    claims.Add(adminClaim);
            //}

            //var userIdentity = new ClaimsIdentity(claims, "login");
            
            //var newToken = _jwtSecurityTokenHandler.CreateJwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Issuer"],userIdentity,DateTime.Now,DateTime.Now.AddMinutes(10),DateTime.Now,_signingCredentials,)
            //var jwt = new JwtSecurityToken(_jwtHeader, payload);
            //var token = _jwtSecurityTokenHandler.WriteToken(jwt);

            // // // //

            var claims = new List<Claim>() {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("UserId", user.Id.ToString())
            };

            if (user.IsAdministrator)
            {
                var adminClaim = new Claim(ClaimTypes.Role, "Administrator");
                claims.Add(adminClaim);
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(int.Parse(_configuration["Jwt:ExpiryMinutes"])),
                signingCredentials: creds);

            var secureToken =  new JwtSecurityTokenHandler().WriteToken(token);

            return new JsonWebToken
            {
                AccessToken = secureToken
            };
        }
    }
}