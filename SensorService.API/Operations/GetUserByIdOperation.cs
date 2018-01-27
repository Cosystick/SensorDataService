using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.DTOs;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetUserByIdOperation : OperationBase<UserIdDTO>, IGetUserByIdOperation
    {
        public GetUserByIdOperation(SensorContext context, IHttpContextAccessor httpContextAccessor, INoAuthorization<UserIdDTO> authorization) : base(context, httpContextAccessor, authorization)
        {
        }

        public override IActionResult OperationBody(UserIdDTO userIdDTO)
        {
            var existingUser = Context.Users.SingleOrDefault(u => u.Id == userIdDTO.Id);
            if (existingUser == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult(existingUser);
        }
    }
}
