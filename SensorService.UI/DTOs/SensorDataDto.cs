using System;

namespace SensorService.UI.DTOs
{
    public class SensorDataDto
    {
        public int Id { get; set; }
        public DateTime Created { get; set; }
        public long Value { get; set; }
    }
}
