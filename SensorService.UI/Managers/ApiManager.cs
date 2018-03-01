using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using SensorService.Shared.Dtos;
using SensorService.Shared.Wrappers;

namespace SensorService.UI.Managers
{
    internal class ApiManager : ApiManagerBase, IApiManager
    {

        protected const string GetDevicesEndpoint = "api/devices/";
        protected const string GetDeviceByIdEndpoint = "api/devices/{0}";
        protected const string UsersEndpoint = "api/users";
        protected const string GetUserByIdEndpoint = "api/users/{0}";
        protected const string GetDevicesByUserEndpoint = "api/Devices/user/{0}";


        public ApiManager(IConfiguration configuration,
            IHttpClientFactory httpClientFactory,
            ISessionManager sessionManager,
            IHttpContextAccessor httpContextAccessor)
            : base(configuration, httpClientFactory, sessionManager, httpContextAccessor)
        {
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

        public async Task<List<UserDto>> GetUsers()
        {
            return await GetData<List<UserDto>>(UsersEndpoint);
        }

        public async Task<UserDto> GetUserById(int id)
        {
            var endpoint = string.Format(GetUserByIdEndpoint, id);
            return await GetData<UserDto>(endpoint);
        }

        public async Task<List<DeviceDto>> GetDevicesByUser(int userId)
        {
            var endpoint = string.Format(GetDevicesByUserEndpoint, userId);
            return await GetData<List<DeviceDto>>(endpoint);
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            return await PostData<UserDto, UserDto>(userDto, UsersEndpoint);
        }

        public async Task<UserDto> UpdateUser(UserDto userDto)
        {
            return await PutData<UserDto, UserDto>(userDto, UsersEndpoint);
        }
    }
}


