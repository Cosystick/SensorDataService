using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.CodeAnalysis.CSharp;
using SensorService.UI.DTOs;
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