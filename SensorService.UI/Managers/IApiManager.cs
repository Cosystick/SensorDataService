using System.Collections.Generic;
using System.Threading.Tasks;
using SensorService.UI.DTOs;
using SensorService.UI.Models;

namespace SensorService.UI.Managers
{
    public interface IApiManager
    {
        Task<bool> DoLogin(LoginModel loginModel);
        Task<List<DeviceDto>> GetDevices();
        Task<DeviceDto> GetDeviceById(string id);
        Task DoLogout();
    }
}