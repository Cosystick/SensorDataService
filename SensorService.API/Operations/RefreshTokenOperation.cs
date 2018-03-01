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
    public class RefreshTokenOperation : OperationBase<RefreshTokenDto>, IRefreshTokenOperation
    {
        private readonly IAccountService _accountService;

        public RefreshTokenOperation(SensorContext context,
            IAccountService accountService,
            IHttpContextAccessor httpContextAccessor,
            INoAuthorization<RefreshTokenDto> authorization)
            : base(context, httpContextAccessor, authorization)
        {
            _accountService = accountService;
        }

        public override IActionResult OperationBody(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var token = _accountService.RefreshAccessToken(refreshTokenDto.Token);
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
