using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Extensions;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public interface ILoginUserOperation : IOperation<LoginDto>
    {
    }

    public class LoginUserOperation : OperationBase<LoginDto>, ILoginUserOperation
    {
        private readonly IUserQueries _userQueries;

        public LoginUserOperation(SensorContext context, IHttpContextAccessor httpContextAccessor, INoAuthorization<LoginDto> authorization, IUserQueries userQueries) : base(context, httpContextAccessor, authorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody(LoginDto loginDto)
        {
            var user = _userQueries.Login(loginDto.UserName, loginDto.Password);
            if (user == null)
            {
                return new NotFoundResult();
            }

            return new OkObjectResult<UserDto, User>(user);
        }
    }
}

