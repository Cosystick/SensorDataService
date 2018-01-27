using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.DTOs;
using SensorService.API.Operations;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class TokenController : Controller
    {
        private readonly IGenerateTokenOperation _generateTokenOperation;
        private readonly IGetUsersOperation _getUsersOperation;

        public TokenController(IGenerateTokenOperation generateTokenOperation,IGetUsersOperation getUsersOperation)
        {
            _generateTokenOperation = generateTokenOperation;
            _getUsersOperation = getUsersOperation;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult GenerateToken([FromBody] LoginDTO login)
        {
            return _generateTokenOperation.Execute(login);
        }
    }
}