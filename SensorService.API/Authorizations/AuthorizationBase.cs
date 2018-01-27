using System;
using SensorService.API.Models;

namespace SensorService.API.Authorizations
{
    public abstract class AuthorizationBase : IAuthorization
    {
        public void Authorize(User currentUser)
        {
            if (!AuthorizeBody(currentUser))
            {
                throw new UnauthorizedAccessException();
            }
        }

        public abstract bool AuthorizeBody(User currentUser);
    }

    public abstract class AuthorizationBase<T> : IAuthorization<T>
    {

        public void Authorize(User currentUser,T input)
        {
            if (!AuthorizeBody(currentUser,input))
            {
                throw new UnauthorizedAccessException();
            }
        }

        public abstract bool AuthorizeBody(User currentUser,T input);
    }
}
