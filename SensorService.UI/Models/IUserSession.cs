namespace SensorService.UI.Models
{
    public interface IUserSession
    {
        string UserName { get; set; }
        string BearerToken { get; set; }
        bool IsAdministrator { get; set; }
    }
}