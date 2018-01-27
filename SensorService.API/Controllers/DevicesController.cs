using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.DTOs;
using SensorService.API.Operations;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class DevicesController : Controller
    {
        private readonly IGetDevicesOperation _getDevicesOperation;
        private readonly IGetDeviceByIdOperation _getDeviceByIdOperation;
        private readonly ISendSensorDataOperation _sendSensorDataOperation;
        private readonly IUpdateDeviceOperation _updateDeviceOperation;

        public DevicesController(IGetDevicesOperation getDevicesOperation,
                                 IGetDeviceByIdOperation getDeviceByIdOperation,
                                 ISendSensorDataOperation sendSensorDataOperation,
                                 IUpdateDeviceOperation updateDeviceOperation)
        {
            _getDevicesOperation = getDevicesOperation;
            _getDeviceByIdOperation = getDeviceByIdOperation;
            _sendSensorDataOperation = sendSensorDataOperation;
            _updateDeviceOperation = updateDeviceOperation;
        }

        // GET api/values
        
        [HttpGet,Authorize]
        public IActionResult Get()
        {
            return _getDevicesOperation.Execute();
        }

        [HttpGet("{id}"), Authorize]
        public IActionResult GetById(string id)
        {
            return _getDeviceByIdOperation.Execute(id);
        }

        [HttpPost, Authorize]
        public IActionResult SendSensorData([FromBody] DeviceDataDTO deviceDataDTO)
        {
            return _sendSensorDataOperation.Execute(deviceDataDTO);
        }

        [HttpPut, Authorize]
        public IActionResult Update([FromBody] DeviceDTO deviceDTO)
        {
            return _updateDeviceOperation.Execute(deviceDTO);
        }
    }
}