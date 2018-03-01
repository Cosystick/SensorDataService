using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SensorService.API.Queries;
using SensorService.API.Services;

namespace SensorService.API.Middleware
{
    public class AccountServiceMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceProvider _serviceProvider;

        public AccountServiceMiddleware(RequestDelegate next, IServiceProvider serviceProvider)
        {
            _next = next;
            _serviceProvider = serviceProvider;
        }

        public async Task Invoke(HttpContext context,
            IAccountService accountService, 
            IUserQueries userQueries,
            ITokenQueries tokenQueries)
        {
            accountService.UserQueries = userQueries;
            accountService.TokenQueries = tokenQueries;
            await _next(context);
        }
    }
}