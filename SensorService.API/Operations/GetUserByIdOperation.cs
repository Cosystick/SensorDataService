using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Extensions;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class GetUserByIdOperation : OperationBase<UserIdDto>, IGetUserByIdOperation
    {
        private readonly IUserQueries _userQueries;

        public GetUserByIdOperation(SensorContext context, 
            IHttpContextAccessor httpContextAccessor, 
            IUserQueries userQueries,
            INoAuthorization<UserIdDto> authorization) 
            : base(context, httpContextAccessor, authorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody(UserIdDto UserIdDto)
        {
            var existingUser = _userQueries.GetById(UserIdDto.Id);
            if (existingUser == null)
            {
                return new NotFoundResult();
            }
            return new OkObjectResult<UserDto,User>(existingUser);
        }
    }
}
