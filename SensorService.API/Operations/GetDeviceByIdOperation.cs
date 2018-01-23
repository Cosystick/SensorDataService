using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetDeviceByIdOperation : OperationBase<string>, IGetDeviceByIdOperation
    {
        public GetDeviceByIdOperation(SensorContext context) : base(context)
        {
        }

        public override IActionResult OperationBody(string id)
        {

            var device = Context.Devices.Include(d => d.Sensors)
                                         .ThenInclude(s => s.Data)
                                         .SingleOrDefault(d => d.Id == id);

            if (device == null)
            {
                return new NotFoundObjectResult(id);
            }
            return new ObjectResult(device);
        }
    }
}