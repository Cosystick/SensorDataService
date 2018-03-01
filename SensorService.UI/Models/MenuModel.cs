using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SensorService.Shared.Dtos;

namespace SensorService.UI.Models
{
    public class MenuModel
    {
        public UserDto User { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
