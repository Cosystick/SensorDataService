using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class UpdateDeviceOperation : OperationBase<DeviceDTO>, IUpdateDeviceOperation
    {
        public UpdateDeviceOperation(SensorContext context) : base(context)
        {
        }

        public override IActionResult OperationBody(DeviceDTO deviceDTO)
        {
            var device = Context.Devices.SingleOrDefault(d => d.Id == deviceDTO.Id);

            if (device == null)
            {
                return new BadRequestResult();
            }

            device.Name = deviceDTO.Name;
            device.IsVisible = deviceDTO.IsVisible;
            device.Created = deviceDTO.Created;

            Context.Update(device);
            Context.SaveChanges();
            return new ObjectResult(device);
        }
    }
}