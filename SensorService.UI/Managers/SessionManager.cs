using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using SensorService.Shared.Dtos;
using SensorService.UI.Extensions;

namespace SensorService.UI.Managers
{
    public class SessionManager : ISessionManager
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SessionManager(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated => _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated;

        public string UserName => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        public string Email => _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        public bool IsAdministrator => CurrentUser != null ? CurrentUser.IsAdministrator : false;//_httpContextAccessor.HttpContext.User.IsInRole("Administrator");

        public bool IsSessionAvailable => _httpContextAccessor.HttpContext.Session.IsAvailable;

        public UserDto CurrentUser
        {
            get
            {
                UserDto currentUser = null;
                if (IsSessionAvailable)
                {
                    currentUser = GetSessionValue<UserDto>("CurrentUser");
                }

                return currentUser;
            }
            set
            {
                if (IsSessionAvailable)
                {
                    SetSessionValue<UserDto>("CurrentUser", value);
                }
            }
        }

        public T GetSessionValue<T>(string key)
        {
            return _httpContextAccessor.HttpContext.Session.Get<T>(key);
        }

        public void SetSessionValue<T>(string key, T value)
        {
            _httpContextAccessor.HttpContext.Session.Set<T>(key, value);
        }
    }

    public interface ISessionManager
    {
        bool IsAuthenticated { get; }
        string UserName { get; }
        string Email { get; }
        bool IsAdministrator { get; }
        bool IsSessionAvailable { get; }
        UserDto CurrentUser { get; set; }
        T GetSessionValue<T>(string key);
        void SetSessionValue<T>(string key, T value);
    }
}
