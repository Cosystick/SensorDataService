using System.Collections.Generic;

namespace SensorService.Shared.Dtos
{
    public class UpdateDeviceDataDto
    {
        public UpdateDeviceDataDto()
        {
            SensorData = new List<UpdateSensorDataDto>();
        }

        public string DeviceId { get; set; }
        public string Name { get; set; }
        public List<UpdateSensorDataDto> SensorData { get; set; }
    }
}
