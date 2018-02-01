using System;
using System.Collections.Generic;
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
using Newtonsoft.Json;
using SensorService.UI.DTOs;
using SensorService.UI.Models;
using SensorService.UI.Wrappers;

namespace SensorService.UI.Managers
{
    public class ApiManager : IApiManager
    {
        private const string TokenEndpoint = "api/token";
        private const string GetDevicesEndpoint = "api/devices";
        private const string GetDeviceByIdEndpoint = "api/devices/{0}";

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Uri _baseAddress;

        public ApiManager(IConfiguration configuration,
                          IHttpClientFactory httpClientFactory,
                          IHttpContextAccessor httpContextAccessor)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _baseAddress = new Uri(configuration["Api:BaseUri"]);
        }

        public async Task<DeviceDto> GetDeviceById(string id)
        {
            var endpoint = string.Format(GetDeviceByIdEndpoint, id);
            return await GetData<DeviceDto>(endpoint);
        }

        public async Task<List<DeviceDto>> GetDevices()
        {
            return await GetData<List<DeviceDto>>(GetDevicesEndpoint);
        }

        public async Task<bool> DoLogin(LoginModel loginModel)
        {
            try
            {
                var tokenDto = await GetToken(loginModel);
                await SetUserToken(loginModel, tokenDto);
                return true;
            }
            catch (UnauthorizedAccessException)
            {
                return false;
            }
        }

        public async Task DoLogout()
        {
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        private async Task<TResult> GetData<TResult>(string endpoint,
                                                     bool addAuthorizationToken = true)
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
                            throw new UnauthorizedAccessException();
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

        private async Task<TResult> PostData<TResult, TRequest>(TRequest request,
                                                                string endpoint,
                                                                bool addAuthorizationToken = true)
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
                            throw new UnauthorizedAccessException();
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

        private async Task<TokenDto> GetToken(LoginModel loginModel)
        {
            return await PostData<TokenDto, LoginModel>(loginModel, TokenEndpoint, false);
        }

        private async Task SetUserToken(LoginModel loginModel, TokenDto token)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, loginModel.UserName),
                new Claim("Token", token.Token)
            };

            var userIdentity = new ClaimsIdentity(claims, "login");

            var principal = new ClaimsPrincipal(userIdentity);
            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        }

        private string SessionToken
        {
            get
            {
                var identity = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;
                return identity.Claims.FirstOrDefault(c => c.Type == "Token")?.Value;
            }
        }
    }
}
