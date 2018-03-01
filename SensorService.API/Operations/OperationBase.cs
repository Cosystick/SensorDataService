using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Authorizations;
using SensorService.API.Models;

namespace SensorService.API.Operations
{
    public abstract class OperationBase<T> : IOperation<T>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorization<T> _authorization;

        protected OperationBase(SensorContext context,
                                IHttpContextAccessor httpContextAccessor,
                                IAuthorization<T> authorization)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorization = authorization;
            Context = context;
        }

        public SensorContext Context { get; set; }
        public int CurrentUserId { get; private set; }
        public User CurrentUser { get; private set; }

        public abstract IActionResult OperationBody(T input);

        public IActionResult Execute(T input)
        {
            SetCurrentUser();
            try
            {
                _authorization.Authorize(CurrentUser, input);
            }
            catch (UnauthorizedAccessException)
            {
                return new UnauthorizedResult();
            }
            return OperationBody(input);
        }

        private void SetCurrentUser()
        {
            var userString = _httpContextAccessor
                .HttpContext
                .User
                .FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userString))
            {
                CurrentUserId = int.Parse(userString);
                CurrentUser = Context.Users.SingleOrDefault(u => u.Id == CurrentUserId);
            }
        }
    }

    public abstract class OperationBase : IOperation
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAuthorization _authorization;

        protected OperationBase(SensorContext context, IHttpContextAccessor httpContextAccessor,
            IAuthorization authorization)
        {
            _httpContextAccessor = httpContextAccessor;
            _authorization = authorization;
            Context = context;
        }

        public int CurrentUserId { get; private set; }
        public User CurrentUser { get; private set; }
        public SensorContext Context { get; set; }

        public abstract IActionResult OperationBody();

        public IActionResult Execute()
        {
            SetCurrentUser();
            try
            {
                _authorization.Authorize(CurrentUser);
            }
            catch (UnauthorizedAccessException)
            {
                return new UnauthorizedResult();
            }
            return OperationBody();
        }

        private void SetCurrentUser()
        {
            var userString = _httpContextAccessor.HttpContext.User.FindFirstValue("UserId");
            if (!string.IsNullOrEmpty(userString))
            {
                CurrentUserId = int.Parse(userString);
                CurrentUser = Context.Users.SingleOrDefault(u => u.Id == CurrentUserId);
            }
        }
    }
}