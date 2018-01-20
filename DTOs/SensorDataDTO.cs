namespace SensorAPI.DTOs
{
    public class SensorDataDTO
    {
        public string SensorKey { get; set; }        
        public int SensorType { get; set; }
        public long Value { get; set; }
    }
}