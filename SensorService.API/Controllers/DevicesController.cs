using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.DTOs;
using SensorService.API.Models;
using SensorService.API.Operations;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly SensorContext _context;
        private readonly IGetDevicesOperation _getDevicesOperation;
        private readonly IGetDeviceByIdOperation _getDeviceByIdOperation;
        private readonly ISendSensorDataOperation _sendSensorDataOperation;
        private readonly IUpdateDeviceOperation _updateDeviceOperation;

        public DevicesController(SensorContext context,
                                 IGetDevicesOperation getDevicesOperation,
                                 IGetDeviceByIdOperation getDeviceByIdOperation,
                                 ISendSensorDataOperation sendSensorDataOperation,
                                 IUpdateDeviceOperation updateDeviceOperation)
        {
            _context = context;
            _getDevicesOperation = getDevicesOperation;
            _getDeviceByIdOperation = getDeviceByIdOperation;
            _sendSensorDataOperation = sendSensorDataOperation;
            _updateDeviceOperation = updateDeviceOperation;
            if (_context.Devices.Count() == 0)
            {
                var testDevice = new Device { Id = "AGUID", Name = "The Flower" };
                var humiditySensor = new Sensor { SensorKey = "1", SensorType = 1 };
                var data = new SensorData(30);
                humiditySensor.Data.Add(data);
                var lightSensor = new Sensor { SensorKey = "2", SensorType = 2 };
                var temperatureSensor = new Sensor { SensorKey = "3", SensorType = 3 };
                testDevice.Sensors.Add(humiditySensor);
                testDevice.Sensors.Add(lightSensor);
                testDevice.Sensors.Add(temperatureSensor);
                _context.Add(testDevice);
                _context.SaveChanges();
            }
        }

        // GET api/values
        [HttpGet]
        public IActionResult Get()
        {
            return _getDevicesOperation.Execute();
        }

        [HttpGet("{id}")]
        public IActionResult GetById(string id)
        {
            return _getDeviceByIdOperation.Execute(id);
        }

        [HttpPost]
        public IActionResult SendSensorData([FromBody] DeviceDataDTO deviceDataDTO)
        {
            return _sendSensorDataOperation.Execute(deviceDataDTO);            
        }

        [HttpPut]
        public IActionResult Update([FromBody] DeviceDTO deviceDTO) {
            return _updateDeviceOperation.Execute(deviceDTO);
        }
    }
}