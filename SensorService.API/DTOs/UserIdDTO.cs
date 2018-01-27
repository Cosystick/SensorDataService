namespace SensorService.API.DTOs
{
    public class UserIdDTO
    {
        public UserIdDTO()
        {
            
        }

        public UserIdDTO(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
