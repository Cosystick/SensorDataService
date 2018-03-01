using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.Operations;
using SensorService.Shared.Dtos;

namespace SensorService.API.Controllers
{
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IGetUsersOperation _getUsersOperation;
        private readonly IInsertUserOperation _insertUserOperation;
        private readonly IUpdateUserOperation _updateUserOperation;
        private readonly IDeleteUserOperation _deleteUserOperation;
        private readonly IGetUserByIdOperation _getUserByIdOperation;
        private readonly ILoginUserOperation _loginUserOperation;

        public UsersController(IGetUsersOperation getUsersOperation,
                               IInsertUserOperation insertUserOperation,
                               IUpdateUserOperation updateUserOperation,
                               IDeleteUserOperation deleteUserOperation,
                               IGetUserByIdOperation getUserByIdOperation,
                               ILoginUserOperation loginUserOperation)
        {
            _getUsersOperation = getUsersOperation;
            _insertUserOperation = insertUserOperation;
            _updateUserOperation = updateUserOperation;
            _deleteUserOperation = deleteUserOperation;
            _getUserByIdOperation = getUserByIdOperation;
            _loginUserOperation = loginUserOperation;
        }

        [HttpGet, Authorize]
        public IActionResult Get() => _getUsersOperation.Execute();

        [HttpPost, Authorize]
        public IActionResult Insert([FromBody] UserDto userDto) => _insertUserOperation.Execute(userDto);

        [HttpPut, Authorize]
        public IActionResult Update([FromBody] UserDto userDto) => _updateUserOperation.Execute(userDto);

        [HttpDelete, Authorize]
        public IActionResult Delete([FromBody] UserIdDto userIdDto) => _deleteUserOperation.Execute(userIdDto);

        [HttpGet("{id}"), Authorize]
        public IActionResult GetById(int id) => _getUserByIdOperation.Execute(new UserIdDto { Id = id });

        [AllowAnonymous]
        [HttpPost,Route("login/")]
        public IActionResult Login([FromBody] LoginDto login)
        {
            return _loginUserOperation.Execute(login);
        }
    }
}