using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Operations;
using SensorService.Shared.Dtos;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly IGetDevicesOperation _getDevicesOperation;
        private readonly IGetDeviceByIdOperation _getDeviceByIdOperation;
        private readonly ISendSensorDataOperation _sendSensorDataOperation;
        private readonly IUpdateDeviceOperation _updateDeviceOperation;
        private readonly IGetDevicesByUserOperation _getDevicesByUserOperation;

        public DevicesController(IGetDevicesOperation getDevicesOperation,
                                 IGetDeviceByIdOperation getDeviceByIdOperation,
                                 ISendSensorDataOperation sendSensorDataOperation,
                                 IUpdateDeviceOperation updateDeviceOperation,
                                 IGetDevicesByUserOperation getDevicesByUserOperation)
        {
            _getDevicesOperation = getDevicesOperation;
            _getDeviceByIdOperation = getDeviceByIdOperation;
            _sendSensorDataOperation = sendSensorDataOperation;
            _updateDeviceOperation = updateDeviceOperation;
            _getDevicesByUserOperation = getDevicesByUserOperation;
        }

        // GET api/values

        [HttpGet, Authorize]
        public IActionResult Get() => _getDevicesOperation.Execute();

        [HttpGet("{id}"), Authorize]
        public IActionResult GetById(string id) => _getDeviceByIdOperation.Execute(id);

        [HttpGet("user/{id}"), Authorize]
        public IActionResult GetByUser(int id) => _getDevicesByUserOperation.Execute(new UserIdDto(id));

        [HttpPost, Authorize]
        public IActionResult SendSensorData([FromBody] UpdateDeviceDataDto updateDeviceDataDto) => _sendSensorDataOperation.Execute(updateDeviceDataDto);

        [HttpPut, Authorize]
        public IActionResult Update([FromBody] DeviceDto deviceDto) => _updateDeviceOperation.Execute(deviceDto);
    }
}