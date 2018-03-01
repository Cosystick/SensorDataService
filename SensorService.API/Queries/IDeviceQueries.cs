using System.Collections.Generic;
using SensorService.API.Models;
using SensorService.Shared.Dtos;

namespace SensorService.API.Queries
{
    public interface IDeviceQueries
    {
        Device GetById(string id);
        List<Device> Get();
        List<Device> GetByUser(int userId);
        Device UpdateDeviceData(int userId, UpdateDeviceDataDto updateDeviceDataDto);
        Device Insert(DeviceDto deviceDto);
        Device Update(DeviceDto deviceDto);
    }
}