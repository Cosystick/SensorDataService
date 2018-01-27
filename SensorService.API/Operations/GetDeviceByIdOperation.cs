using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorService.API.Authorizations;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetDeviceByIdOperation : OperationBase<string>, IGetDeviceByIdOperation
    {
        public GetDeviceByIdOperation(SensorContext context,
                                      IHttpContextAccessor httpContextAccessor,
                                      INoAuthorization<string> noAuthorization)
                                      : base(context, httpContextAccessor, noAuthorization)
        {
        }

        public override IActionResult OperationBody(string id)
        {
            var device = Context.Devices.Include(d => d.Sensors)
                                         .ThenInclude(s => s.Data)
                                         .SingleOrDefault(d => d.Id == id);

            if (device != null && (device.UserId != CurrentUserId && !CurrentUser.IsAdministrator))
            {
                return new UnauthorizedResult();
            }

            return device == null ? new NotFoundObjectResult(id) : new ObjectResult(device);
        }
    }
}