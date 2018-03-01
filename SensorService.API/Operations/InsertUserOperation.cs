using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class InsertUserOperation : OperationBase<UserDto>, IInsertUserOperation
    {
        private readonly IUserQueries _userQueries;

        public InsertUserOperation(SensorContext context,
                                   IHttpContextAccessor httpContextAccessor,
                                   IUserQueries userQueries,
                                   IAdministratorAuthorization<UserDto> administratorAuthorization)
            : base(context, httpContextAccessor, administratorAuthorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody(UserDto UserDto)
        {
            var user = _userQueries.Insert(UserDto);

            if (user == null)
            {
                return new BadRequestObjectResult("Could not insert user, the username might already exist");
            }

            return new OkObjectResult(user);
        }
    }
}
