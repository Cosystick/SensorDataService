using System;
using System.Security.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Services;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class GenerateTokenOperation : OperationBase<LoginDto>, IGenerateTokenOperation
    {
        private readonly IAccountService _accountService;

        public GenerateTokenOperation(SensorContext context,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor,
            INoAuthorization<LoginDto> noAuthorization)
            : base(context, httpContextAccessor, noAuthorization)
        {
            _accountService = accountService;
        }

        public override IActionResult OperationBody(LoginDto loginDto)
        {
            try
            {
                var token = _accountService.SignIn(loginDto.UserName, loginDto.Password);
                return new OkObjectResult(token);
            }
            catch (AuthenticationException)
            {
                return new UnauthorizedResult();
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }
    }
}
