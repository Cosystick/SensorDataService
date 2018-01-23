using System;

namespace SensorService.API.DTOs
{
    public class DeviceDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Created { get; set; }
    }
}