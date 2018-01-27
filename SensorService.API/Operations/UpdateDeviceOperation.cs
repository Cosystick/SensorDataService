using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class UpdateDeviceOperation : OperationBase<DeviceDTO>, IUpdateDeviceOperation
    {
        public UpdateDeviceOperation(SensorContext context, 
                                     IHttpContextAccessor httpContextAccessor,
                                     INoAuthorization<DeviceDTO> noAuthorization) 
                                     : base(context, httpContextAccessor, noAuthorization)
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
            device.UserId = deviceDTO.UserId;
            device.IsVisible = deviceDTO.IsVisible;
            device.Created = deviceDTO.Created;

            Context.Update(device);
            Context.SaveChanges();
            return new ObjectResult(device);
        }
    }
}