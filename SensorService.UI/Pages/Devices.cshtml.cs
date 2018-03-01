using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.Shared.Dtos;
using SensorService.UI.Managers;

namespace SensorService.UI.Pages
{
    public class DevicesModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public DevicesModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }
        public void OnGet()
        {
        }

        public List<DeviceDto> Devices => _apiManager.GetDevices().Result;
    }
}