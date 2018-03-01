using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class SendSensorDataOperation : OperationBase<UpdateDeviceDataDto>, ISendSensorDataOperation
    {
        private readonly IDeviceQueries _deviceQueries;

        public SendSensorDataOperation(SensorContext context, 
                                       IDeviceQueries deviceQueries,
                                       IHttpContextAccessor httpContextAccessor,
                                       INoAuthorization<UpdateDeviceDataDto> noAuthorization) 
                                       : base(context, httpContextAccessor, noAuthorization)
        {
            _deviceQueries = deviceQueries;
        }

        public override IActionResult OperationBody(UpdateDeviceDataDto updateDeviceDataDto)
        {
            if (updateDeviceDataDto?.SensorData == null || !updateDeviceDataDto.SensorData.Any())
            {
                return new BadRequestResult();
            }

            var device = _deviceQueries.UpdateDeviceData(CurrentUserId, updateDeviceDataDto);

            return new OkObjectResult(device);
        }
    }
}