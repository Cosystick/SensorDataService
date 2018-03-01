using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class UpdateDeviceOperation : OperationBase<DeviceDto>, IUpdateDeviceOperation
    {
        private readonly IDeviceQueries _deviceQueries;

        public UpdateDeviceOperation(SensorContext context, 
                                     IDeviceQueries deviceQueries,
                                     IHttpContextAccessor httpContextAccessor,
                                     INoAuthorization<DeviceDto> noAuthorization) 
                                     : base(context, httpContextAccessor, noAuthorization)
        {
            _deviceQueries = deviceQueries;
        }

        public override IActionResult OperationBody(DeviceDto deviceDto)
        {
            var device = _deviceQueries.Update(deviceDto);

            if (device == null)
            {
                return new BadRequestResult();
            }

            return new OkResult();
        }
    }
}