namespace SensorService.UI.Models
{
    public class UserSession : IUserSession
    {
        public string UserName { get; set; }
        public string BearerToken { get; set; }
        public bool IsAdministrator { get; set; }
    }
}
