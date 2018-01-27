using SensorService.API.Models;

namespace SensorService.API.Authorizations
{
    public class NoAuthorization : AuthorizationBase, INoAuthorization
    {
        
        public override bool AuthorizeBody(User currentUser)
        {
            return true; // Always ok
        }
    }

    public class NoAuthorization<T> : AuthorizationBase<T>, INoAuthorization<T>
    {
        public override bool AuthorizeBody(User currentUser, T input)
        {
            return true; // Always ok
        }
    }
}
