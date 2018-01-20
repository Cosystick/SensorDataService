using System.Collections.Generic;

namespace SensorAPI.DTOs
{
    public class DeviceDataDTO
    {
        public DeviceDataDTO()
        {
            SensorData = new List<SensorDataDTO>();
        }

        public string DeviceId { get; set; }
        public List<SensorDataDTO> SensorData { get; set; }
    }
}
