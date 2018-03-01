using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Identity;
using SensorService.Api.Services;
using SensorService.API.Models;
using SensorService.API.Queries;

namespace SensorService.API.Services
{
    public class AccountService : IAccountService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IJwtHandler _jwtHandler;
        private readonly IPasswordHasher<User> _passwordHasher;

        public AccountService(IServiceProvider serviceProvider,IJwtHandler jwtHandler,
            IPasswordHasher<User> passwordHasher)
        {
            _serviceProvider = serviceProvider;
            _jwtHandler = jwtHandler;
            _passwordHasher = passwordHasher;
        }

        public IUserQueries UserQueries { get; set; }

        public ITokenQueries TokenQueries { get; set; }

        public JsonWebToken SignIn(string username, string password)
        {
            var user = UserQueries.Login(username, password);
            if (user == null)
            {
                throw new AuthenticationException("Invalid credentials.");
            }
            var jwt = _jwtHandler.Create(user);
            var refreshToken = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString())
                .Replace("+", string.Empty)
                .Replace("=", string.Empty)
                .Replace("/", string.Empty);
            jwt.RefreshToken = refreshToken;

            TokenQueries.SaveRefreshToken(new RefreshToken
            {
                Username = user.UserName,
                UserId = user.Id,
                Token = refreshToken
            }
            );

            return jwt;
        }

        public JsonWebToken RefreshAccessToken(string token)
        {
            var refreshToken = TokenQueries.GetRefreshToken(token);
            if (refreshToken == null)
            {
                throw new AuthenticationException("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new AuthenticationException("Refresh token was revoked");
            }

            var user = UserQueries.GetById(refreshToken.UserId);
            if (user == null)
            {
                throw new AuthenticationException("User not found");
            }
            var jwt = _jwtHandler.Create(user);
            jwt.RefreshToken = refreshToken.Token;

            return jwt;
        }

        public void RevokeRefreshToken(string token)
        {
            TokenQueries.RevokeRefreshToken(token);
        }
    }
}
