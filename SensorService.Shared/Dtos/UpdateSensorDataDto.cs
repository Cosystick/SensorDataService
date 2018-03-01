namespace SensorService.Shared.Dtos
{
    public class UpdateSensorDataDto
    {
        public string SensorKey { get; set; }
        public int SensorType { get; set; }
        public long Value { get; set; }
    }
}
