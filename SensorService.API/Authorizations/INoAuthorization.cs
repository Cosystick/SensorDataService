namespace SensorService.API.Authorizations
{
    public interface INoAuthorization : IAuthorization { }

    public interface INoAuthorization<T> : IAuthorization<T> { }
}
