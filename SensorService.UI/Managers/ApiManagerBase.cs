using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using SensorService.Shared.Dtos;
using SensorService.Shared.Wrappers;
using SensorService.UI.Models;

namespace SensorService.UI.Managers
{
    public interface IApiManagerBase
    {
        Task<bool> DoLogin(LoginModel loginModel);
        Task DoLogout();
    }

    internal abstract class ApiManagerBase : IApiManagerBase
    {
        private const string AccessTokenClaimType = "AccessToken";
        private const string RefreshTokenClaimType = "RefreshToken";
        private const string TokenEndpoint = "api/token";
        private const string RefreshAccessTokenEndpoint = "api/token/{0}/token";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ISessionManager _sessionManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Uri _baseAddress;
        private readonly SymmetricSecurityKey _issuerSigningKey;
        private readonly string _issuer;


        protected ApiManagerBase(IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ISessionManager sessionManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _sessionManager = sessionManager;
            _httpContextAccessor = httpContextAccessor;
            _baseAddress = new Uri(configuration["Api:BaseUri"]);
            _issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            _issuer = configuration["Jwt:Issuer"];
        }

        public async Task<bool> DoLogin(LoginModel loginModel)
        {
            try
            {
                var tokenDto = await GetToken(loginModel);
                var userDto = GetUserInfoFromToken(tokenDto);
                await SetUserToken(userDto, tokenDto);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task DoLogout()
        {
            _sessionManager.CurrentUser = null;
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        protected async Task<TResult> GetData<TResult>(string endpoint,
                                                     bool addAuthorizationToken = true, bool doRetry = true)
        {
            using (var client = _httpClientFactory.Create(_baseAddress))
            {
                if (addAuthorizationToken)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SessionToken);
                }

                var httpResponse = await client.GetAsync(endpoint);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    switch (httpResponse.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            if (!doRetry)
                            {
                                throw new UnauthorizedAccessException();
                            }

                            await GetNewAccessToken();
                            return await GetData<TResult>(endpoint,
                                                          addAuthorizationToken,
                                                          false);
                        case HttpStatusCode.NotFound:
                            throw new FileNotFoundException();
                        default:
                            var exception = new Exception($"Resource server returned an error. StatusCode : {httpResponse.StatusCode}");
                            exception.Data.Add("StatusCode", httpResponse.StatusCode);
                            throw exception;
                    }
                }

                return await httpResponse.Content.ReadAsAsync<TResult>();
            }
        }

        protected async Task<TResult> PutData<TResult, TRequest>(TRequest request, 
            string endpoint,
            bool addAuthorizationToken = true, 
            bool doRetry = true)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = _httpClientFactory.Create(_baseAddress))
            {
                if (addAuthorizationToken)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SessionToken);
                }

                var httpResponse = await client.PutAsync(endpoint, content);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    switch (httpResponse.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            if (!doRetry)
                            {
                                throw new UnauthorizedAccessException();
                            }

                            await GetNewAccessToken();
                            return await PutData<TResult, TRequest>(request,
                                endpoint,
                                addAuthorizationToken,
                                false);
                        case HttpStatusCode.NotFound:
                            throw new FileNotFoundException();
                        default:
                            var exception = new Exception($"Resource server returned an error. StatusCode : {httpResponse.StatusCode}");
                            exception.Data.Add("StatusCode", httpResponse.StatusCode);
                            throw exception;
                    }
                }

                return await httpResponse.Content.ReadAsAsync<TResult>();
            }
        }

        protected async Task<TResult> PostData<TResult, TRequest>(TRequest request,
                                                                string endpoint,
                                                                bool addAuthorizationToken = true,
                                                                bool doRetry = true)
        {
            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = _httpClientFactory.Create(_baseAddress))
            {
                if (addAuthorizationToken)
                {
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + SessionToken);
                }

                var httpResponse = await client.PostAsync(endpoint, content);
                if (!httpResponse.IsSuccessStatusCode)
                {
                    switch (httpResponse.StatusCode)
                    {
                        case HttpStatusCode.Unauthorized:
                            if (!doRetry)
                            {
                                throw new UnauthorizedAccessException();
                            }

                            await GetNewAccessToken();
                            return await PostData<TResult, TRequest>(request,
                                                                     endpoint,
                                                                     addAuthorizationToken,
                                                                     false);
                        case HttpStatusCode.NotFound:
                            throw new FileNotFoundException();
                        default:
                            var exception = new Exception($"Resource server returned an error. StatusCode : {httpResponse.StatusCode}");
                            exception.Data.Add("StatusCode", httpResponse.StatusCode);
                            throw exception;
                    }
                }

                return await httpResponse.Content.ReadAsAsync<TResult>();
            }
        }

        protected async Task GetNewAccessToken()
        {
            try
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                var accessTokenClaim = identity.Claims.Single(c => c.Type == AccessTokenClaimType);
                var refreshTokenClaim = identity.Claims.Single(c => c.Type == RefreshTokenClaimType);
                var endpoint = string.Format(RefreshAccessTokenEndpoint, refreshTokenClaim.Value);
                var token = await GetData<TokenDto>(endpoint, false, false);

                identity.RemoveClaim(accessTokenClaim);
                identity.RemoveClaim(refreshTokenClaim);

                accessTokenClaim = new Claim(AccessTokenClaimType, token.AccessToken);
                refreshTokenClaim = new Claim(RefreshTokenClaimType, token.RefreshToken);
                identity.AddClaim(accessTokenClaim);
                identity.AddClaim(refreshTokenClaim);
            }
            catch
            {
                throw new UnauthorizedAccessException();
            }
        }

        protected async Task<TokenDto> GetToken(LoginModel loginModel)
        {
            return await PostData<TokenDto, LoginModel>(loginModel, TokenEndpoint, false);
        }

        protected async Task SetUserToken(UserDto userDto, TokenDto token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.UserName),
                new Claim(ClaimTypes.Email, userDto.Email),
                new Claim(AccessTokenClaimType, token.AccessToken),
                new Claim(RefreshTokenClaimType, token.RefreshToken)
            };

            if (userDto.IsAdministrator)
            {
                var adminClaim = new Claim(ClaimTypes.Role, "Administrator");
                claims.Add(adminClaim);
            }

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);
            _sessionManager.CurrentUser = userDto;
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        protected string SessionToken
        {
            get
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                return identity.Claims.FirstOrDefault(c => c.Type == AccessTokenClaimType)?.Value;
            }
        }

        protected string RefreshToken
        {
            get
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                return identity.Claims.FirstOrDefault(c => c.Type == RefreshTokenClaimType)?.Value;
            }
        }

        private UserDto GetUserInfoFromToken(TokenDto tokenDto)
        {
            var tokenValidationParameters = new TokenValidationParameters()
            {
                ValidAudiences = new string[]
                {
                    _issuer
                },
                ValidIssuers = new string[]
                {
                    _issuer
                },
                IssuerSigningKey = _issuerSigningKey
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var claimsPrincipal = tokenHandler.ValidateToken(tokenDto.AccessToken,
                tokenValidationParameters, out var _);

            var userDto = new UserDto
            {
                UserName = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
                Email = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
                IsAdministrator = claimsPrincipal.IsInRole("Administrator")
            };
            return userDto;
        }
    }
}
