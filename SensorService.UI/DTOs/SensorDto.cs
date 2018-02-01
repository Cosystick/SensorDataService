using System.Collections.Generic;

namespace SensorService.UI.DTOs
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string SensorKey { get; set; }
        public int SensorType { get; set; }
        public List<SensorDataDto> Data { get; set; }
    }
}
