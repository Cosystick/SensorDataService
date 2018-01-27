using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public class GetUsersOperation : OperationBase, IGetUsersOperation
    {
        public GetUsersOperation(SensorContext context, 
                                 IHttpContextAccessor httpContextAccessor,
                                 INoAuthorization noAuthorization) 
                                 : base(context, httpContextAccessor, noAuthorization)
        {
        }

        public override IActionResult OperationBody()
        {
            return new OkObjectResult(Context.Users.ToList());
        }
    }
}
