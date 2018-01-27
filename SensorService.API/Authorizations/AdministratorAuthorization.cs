using SensorService.API.Models;

namespace SensorService.API.Authorizations
{
    public class AdministratorAuthorization : AuthorizationBase, IAdministratorAuthorization
    {
        public override bool AuthorizeBody(User currentUser)
        {
            return currentUser.IsAdministrator;
        }
    }

    public class AdministratorAuthorization<T> : AuthorizationBase<T>, IAdministratorAuthorization<T>
    {
        public override bool AuthorizeBody(User currentUser, T input)
        {
            return currentUser.IsAdministrator;
        }
    }
}
