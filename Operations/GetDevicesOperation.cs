using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SensorAPI.Models;

namespace SensorAPI.Operations
{
    public class GetDevicesOperation : OperationBase, IGetDevicesOperation
    {

        public GetDevicesOperation(SensorContext context) : base(context)
        {
        }

        public override IActionResult OperationBody()
        {
            var devices = Context.Devices.Include(d => d.Sensors).ToList();
            return new ObjectResult(devices);
        }
    }
}