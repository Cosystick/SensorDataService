using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SensorService.API.DTOs;
using SensorService.API.Operations;

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

        public UsersController(IGetUsersOperation getUsersOperation,
                               IInsertUserOperation insertUserOperation,
                               IUpdateUserOperation updateUserOperation,
                               IDeleteUserOperation deleteUserOperation,
                               IGetUserByIdOperation getUserByIdOperation)
        {
            _getUsersOperation = getUsersOperation;
            _insertUserOperation = insertUserOperation;
            _updateUserOperation = updateUserOperation;
            _deleteUserOperation = deleteUserOperation;
            _getUserByIdOperation = getUserByIdOperation;
        }

        [HttpGet, Authorize]
        public IActionResult Get() => _getUsersOperation.Execute();

        [HttpPost, Authorize]
        public IActionResult Insert([FromBody] UserDTO userDTO) => _insertUserOperation.Execute(userDTO);

        [HttpPut, Authorize]
        public IActionResult Update([FromBody] UserDTO userDTO) => _updateUserOperation.Execute(userDTO);

        [HttpDelete, Authorize]
        public IActionResult Delete([FromBody] UserIdDTO userIdDTO) => _deleteUserOperation.Execute(userIdDTO);

        [HttpGet("{id}"), Authorize]
        public IActionResult GetById(int id) => _getUserByIdOperation.Execute(new UserIdDTO { Id = id });
    }
}