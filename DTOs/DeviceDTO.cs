using System;

namespace SensorAPI.DTOs
{
    public class DeviceDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Created { get; set; }
    }
}