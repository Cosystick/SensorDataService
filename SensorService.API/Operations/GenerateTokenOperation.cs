using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GenerateTokenOperation : OperationBase<LoginDTO>, IGenerateTokenOperation
    {
        private readonly IConfiguration _configuration;

        public GenerateTokenOperation(SensorContext context, 
                                      IConfiguration configuration, 
                                      IHttpContextAccessor httpContextAccessor,
                                      INoAuthorization<LoginDTO> noAuthorization) 
                                      : base(context, httpContextAccessor, noAuthorization)
        {
            _configuration = configuration;
        }

        public override IActionResult OperationBody(LoginDTO login)
        {
            var user = Authenticate(login);

            if (user != null)
            {
                var tokenString = BuildToken(user);
                return new OkObjectResult(new { token = tokenString });
            }

            return new UnauthorizedResult();
        }

        private User Authenticate(LoginDTO login)
        {
            if (!Context.Users.Any())
            {
                var newUser = new User
                {
                    Email = "jens.stjernstrom@gmail.com",
                    IsAdministrator = true,
                    Password = "password",
                    UserName = "Jens"
                };
                Context.Add(newUser);
                Context.SaveChanges();
            }
            return Context.Users.SingleOrDefault(u => u.UserName.ToLower() == login.UserName.ToLower()
                                                      && u.Password == login.Password);
        }

        private string BuildToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("UserId",user.Id.ToString()), 
            };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddYears(3),
                signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
