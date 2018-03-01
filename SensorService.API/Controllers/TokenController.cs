using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Operations;
using SensorService.Shared.Dtos;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IGenerateTokenOperation _generateTokenOperation;
        private readonly IRefreshTokenOperation _refreshTokenOperation;

        public TokenController(IGenerateTokenOperation generateTokenOperation,
            IRefreshTokenOperation refreshTokenOperation)
        {
            _generateTokenOperation = generateTokenOperation;
            _refreshTokenOperation = refreshTokenOperation;
        }

        [HttpGet("{token}/refresh")]
        [AllowAnonymous]
        public IActionResult RefreshAccessToken(RefreshTokenDto tokenDto)
            => _refreshTokenOperation.Execute(tokenDto);

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GenerateToken([FromBody] LoginDto login)
        {
            return _generateTokenOperation.Execute(login);
        }
    }
}