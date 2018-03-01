using System;
using System.Collections.Generic;

namespace SensorService.Shared.Dtos
{
    public class DeviceDto
    {
        public string Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public List<SensorDto> Sensors { get; set; }
        public bool IsVisible { get; set; }
        public DateTime Created { get; set; }
    }
}
