

using SensorService.Shared.Dtos;

namespace SensorService.UI.Models
{
    public class MenuModel
    {
        public UserDto User { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
