using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using SensorService.UI.DTOs;
using SensorService.UI.Managers;

namespace SensorService.UI.Pages
{
    public class DeviceInfoModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public DeviceInfoModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        public void OnGet([FromQuery] string id)
        {
            Device = _apiManager.GetDeviceById(id).Result;
        }

        public DeviceDto Device { get; private set; }

        public Dictionary<string, string> SensorData
        {
            get
            {
                var dict = new Dictionary<string, string>();
                foreach (var sensor in Device.Sensors)
                {
                    var list = sensor.Data.Select(d => d.Value).ToList();
                    var data = JsonConvert.SerializeObject(list);
                    dict.Add(sensor.SensorKey, data);
                }
                return dict;
            }
        }
    }
}