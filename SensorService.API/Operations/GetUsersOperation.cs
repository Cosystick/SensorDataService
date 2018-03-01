using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;
using SensorService.API.Queries;
using SensorService.Shared.Dtos;

namespace SensorService.API.Operations
{
    public class GetUsersOperation : OperationBase, IGetUsersOperation
    {
        private readonly IUserQueries _userQueries;

        public GetUsersOperation(SensorContext context, 
                                 IHttpContextAccessor httpContextAccessor,
                                 IUserQueries userQueries,
                                 IAdministratorAuthorization noAuthorization) 
                                 : base(context, httpContextAccessor, noAuthorization)
        {
            _userQueries = userQueries;
        }

        public override IActionResult OperationBody()
        {
            var users = _userQueries.Get();
            var result = Mapper.Map <List<UserDto>>(users);
            return new OkObjectResult(result);
        }
    }
}
