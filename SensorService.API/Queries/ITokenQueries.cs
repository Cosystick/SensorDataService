using SensorService.API.Models;

namespace SensorService.API.Queries
{
    public interface ITokenQueries
    {
        RefreshToken GetRefreshToken(string token);
        void SaveRefreshToken(RefreshToken refreshToken);
        void RevokeRefreshToken(string token);
    }
}