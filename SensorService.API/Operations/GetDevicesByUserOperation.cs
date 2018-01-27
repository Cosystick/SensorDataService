using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetDevicesByUserOperation : OperationBase<UserIdDTO>, IGetDevicesByUserOperation
    {
        public GetDevicesByUserOperation(SensorContext context, IHttpContextAccessor httpContextAccessor, INoAuthorization<UserIdDTO> authorization) : base(context, httpContextAccessor, authorization)
        {
        }

        public override IActionResult OperationBody(UserIdDTO userIdDTO)
        {
            if (userIdDTO.Id != CurrentUserId && !CurrentUser.IsAdministrator)
            {
                return new UnauthorizedResult();
            }
            var devices = Context.Devices.Where(d => d.UserId == userIdDTO.Id).ToList();
            return new OkObjectResult(devices);
        }
    }
}
