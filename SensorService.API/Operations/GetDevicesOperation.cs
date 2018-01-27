using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorService.API.Authorizations;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetDevicesOperation : OperationBase, IGetDevicesOperation
    {

        public GetDevicesOperation(SensorContext context,
                                   IHttpContextAccessor httpContextAccessor, 
                                   INoAuthorization noAuthorization)
                                   : base(context, httpContextAccessor, noAuthorization)
        {
        }

        public override IActionResult OperationBody()
        {
            var devices = Context.Devices.Include(d => d.Sensors).ToList();
            return new ObjectResult(devices);
        }
    }
}