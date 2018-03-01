using System;
using System.Linq;
using SensorService.API.Models;

namespace SensorService.API.Queries
{
    public class TokenQueries : ITokenQueries
    {
        private readonly SensorContext _context;

        public TokenQueries(SensorContext context)
        {
            _context = context;
        }

        public RefreshToken GetRefreshToken(string token)
        {
            return _context.RefreshTokens.SingleOrDefault(t => t.Token == token);
        }

        public void SaveRefreshToken(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            _context.SaveChanges();
        }

        public void RevokeRefreshToken(string token)
        {
            var refreshToken = _context.RefreshTokens.SingleOrDefault(t => t.Token == token);
            if (refreshToken == null)
            {
                throw new Exception("Refresh token was not found.");
            }
            if (refreshToken.Revoked)
            {
                throw new Exception("Refresh token was already revoked.");
            }

            refreshToken.Revoked = true;
            _context.Update(refreshToken);
            _context.SaveChanges();
        }
    }
}
