using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class UpdateUserOperation : OperationBase<UserDTO>, IUpdateUserOperation
    {
        public UpdateUserOperation(SensorContext context, 
                                   IHttpContextAccessor httpContextAccessor, 
                                   INoAuthorization<UserDTO> authorization) 
            : base(context, httpContextAccessor, authorization)
        {
        }

        public override IActionResult OperationBody(UserDTO userDTO)
        {
            var existingUser = Context.Users.SingleOrDefault(u => u.Id == userDTO.Id);
            if (existingUser == null)
            {
                return new BadRequestResult();
            }

            if (existingUser.Id != CurrentUserId && !CurrentUser.IsAdministrator)
            {
                return new UnauthorizedResult();
            }

            existingUser.Email = userDTO.Email;
            existingUser.Password = userDTO.Password;
            existingUser.IsAdministrator = userDTO.IsAdministrator;
            Context.Update(existingUser);
            Context.SaveChanges();
            return new OkResult();
        }
    }
}
