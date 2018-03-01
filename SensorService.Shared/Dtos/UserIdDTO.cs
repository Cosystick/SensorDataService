namespace SensorService.Shared.Dtos
{
    public class UserIdDto
    {
        public UserIdDto() { }

        public UserIdDto(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
