using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class GetDevicesOperation : OperationBase, IGetDevicesOperation
    {
        private readonly IDeviceQueries _deviceQueries;

        public GetDevicesOperation(SensorContext context,
                                   IHttpContextAccessor httpContextAccessor, 
                                   IDeviceQueries deviceQueries,
                                   IAdministratorAuthorization noAuthorization)
                                   : base(context, httpContextAccessor, noAuthorization)
        {
            _deviceQueries = deviceQueries;
        }

        public override IActionResult OperationBody()
        {
            var devices = _deviceQueries.Get();
            var result = Mapper.Map<List<DeviceDto>>(devices);
            return new OkObjectResult(result);
        }
    }
}