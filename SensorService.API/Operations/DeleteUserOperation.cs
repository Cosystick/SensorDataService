using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class DeleteUserOperation : OperationBase<UserIdDTO>, IDeleteUserOperation
    {
        public DeleteUserOperation(SensorContext context,
                                   IHttpContextAccessor httpContextAccessor,
                                   IAdministratorAuthorization<UserIdDTO> authorization)
            : base(context, httpContextAccessor, authorization)
        {
        }

        public override IActionResult OperationBody(UserIdDTO userIdDTO)
        {
            var existingUser = Context.Users.SingleOrDefault(u => u.Id == userIdDTO.Id);
            if (existingUser == null)
            {
                return new NotFoundResult();
            }

            var userDevices = Context.Devices.Where(d => d.UserId == userIdDTO.Id).ToList();
            Context.RemoveRange(userDevices);
            Context.Remove(existingUser);
            Context.SaveChanges();
            return new OkResult();
        }
    }
}
