using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Extensions;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class GetDeviceByIdOperation : OperationBase<string>, IGetDeviceByIdOperation
    {
        private readonly IDeviceQueries _deviceQueries;

        public GetDeviceByIdOperation(SensorContext context,
                                      IDeviceQueries deviceQueries,
                                      IHttpContextAccessor httpContextAccessor,
                                      INoAuthorization<string> noAuthorization)
                                      : base(context, httpContextAccessor, noAuthorization)
        {
            _deviceQueries = deviceQueries;
        }

        public override IActionResult OperationBody(string id)
        {
            var device = _deviceQueries.GetById(id);
            if (device != null && (device.UserId != CurrentUserId && !CurrentUser.IsAdministrator))
            {
                return new UnauthorizedResult();
            }

            if (device == null)
            {
                return new NotFoundObjectResult(id);
            }
            else
            {
                return new OkObjectResult<DeviceDto,Device>(device);
            }
        }
    }
}