using System.Collections.Generic;

namespace SensorService.Shared.Dtos
{
    public class SensorDto
    {
        public int Id { get; set; }
        public string SensorKey { get; set; }
        public int SensorType { get; set; }
        public List<SensorDataDto> Data { get; set; }
    }
}
