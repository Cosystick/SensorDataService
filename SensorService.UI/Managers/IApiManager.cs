using System.Collections.Generic;
using System.Threading.Tasks;
using SensorService.Shared.Dtos;
using SensorService.UI.Models;

namespace SensorService.UI.Managers
{
    public interface IApiManager : IApiManagerBase
    {
        Task<DeviceDto> GetDeviceById(string id);
        Task<List<DeviceDto>> GetDevices();
        Task<List<UserDto>> GetUsers();
        Task<UserDto> GetUserById(int id);
        Task<List<DeviceDto>> GetDevicesByUser(int userId);
        Task<UserDto> CreateUser(UserDto userDto);
        Task<UserDto> UpdateUser(UserDto userDto);
    }
}