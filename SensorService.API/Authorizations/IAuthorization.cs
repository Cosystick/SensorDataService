using SensorService.API.Models;

namespace SensorService.API.Authorizations
{
    public interface IAuthorization
    {
        void Authorize(User currentUser);
    }

    public interface IAuthorization<T>
    {
        void Authorize(User currentUser, T input);
    }
}