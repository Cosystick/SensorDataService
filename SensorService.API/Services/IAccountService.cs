using SensorService.API.Models;
using SensorService.API.Queries;

namespace SensorService.API.Services
{
    public interface IAccountService
    {
        IUserQueries UserQueries { get; set; }
        ITokenQueries TokenQueries { get; set; }
        JsonWebToken SignIn(string username, string password);
        JsonWebToken RefreshAccessToken(string token);
        void RevokeRefreshToken(string token);
    }
}