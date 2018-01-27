using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class InsertUserOperation : OperationBase<UserDTO>, IInsertUserOperation
    {
        public InsertUserOperation(SensorContext context,
                                   IHttpContextAccessor httpContextAccessor,
                                   IAdministratorAuthorization<UserDTO> administratorAuthorization)
            : base(context, httpContextAccessor, administratorAuthorization)
        {
        }

        public override IActionResult OperationBody(UserDTO userDTO)
        {
            var existingUser =
                Context.Users.SingleOrDefault(u => u.UserName == userDTO.UserName || u.Email == userDTO.Email);
            if (existingUser != null)
            {
                return new BadRequestObjectResult("User already exists");
            }

            var newUser = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                Password = userDTO.Password,
                IsAdministrator = userDTO.IsAdministrator
            };

            Context.Users.Add(newUser);
            Context.SaveChanges();
            return new OkObjectResult(newUser);
        }
    }
}
