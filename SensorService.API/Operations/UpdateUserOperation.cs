using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class UpdateUserOperation : OperationBase<UserDto>, IUpdateUserOperation
    {
        private readonly IUserQueries _userQueries;

        public UpdateUserOperation(SensorContext context, 
                                   IUserQueries userQueries,
                                   IHttpContextAccessor httpContextAccessor, 
                                   INoAuthorization<UserDto> authorization) 
            : base(context, httpContextAccessor, authorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody(UserDto userDto)
        {
            if (userDto.Id != CurrentUserId && !CurrentUser.IsAdministrator)
            {
                return new UnauthorizedResult();
            }

            var existingUser = _userQueries.Update(userDto);
            if (existingUser == null)
            {
                return new BadRequestResult();
            }
            
            return new OkResult();
        }
    }
}
