using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class DeleteUserOperation : OperationBase<UserIdDto>, IDeleteUserOperation
    {
        private readonly IUserQueries _userQueries;

        public DeleteUserOperation(SensorContext context,
                                   IUserQueries userQueries,
                                   IHttpContextAccessor httpContextAccessor,
                                   IAdministratorAuthorization<UserIdDto> authorization)
            : base(context, httpContextAccessor, authorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody(UserIdDto userIdDto)
        {
            _userQueries.Delete(userIdDto.Id);
            return new OkResult();
        }
    }
}
