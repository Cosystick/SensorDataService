using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;
using SensorService.API.Extensions;

namespace SensorService.API.Operations
{
    public class GetDevicesByUserOperation : OperationBase<UserIdDto>, IGetDevicesByUserOperation
    {
        private readonly IDeviceQueries _deviceQueries;

        public GetDevicesByUserOperation(SensorContext context, 
            IHttpContextAccessor httpContextAccessor, 
            IDeviceQueries deviceQueries,
            INoAuthorization<UserIdDto> authorization) 
            : base(context, httpContextAccessor, authorization)
        {
            _deviceQueries = deviceQueries;
        }

        public override IActionResult OperationBody(UserIdDto userIdDto)
        {
            if (userIdDto.Id != CurrentUserId && !CurrentUser.IsAdministrator)
            {
                return new UnauthorizedResult();
            }
            var devices = Mapper.Map<List<DeviceDto>>(_deviceQueries.GetByUser(userIdDto.Id));
            return new OkObjectResult(devices);
        }
    }
}
