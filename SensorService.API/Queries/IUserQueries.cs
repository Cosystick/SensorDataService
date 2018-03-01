using System.Collections.Generic;
using SensorService.API.Models;
using SensorService.Shared.Dtos;

namespace SensorService.API.Queries
{
    public interface IUserQueries
    {
        User Login(string userName, string password);
        User GetById(int id);
        bool IsAdministrator(int id);
        List<User> Get();
        User Update(UserDto userDto);
        User Insert(UserDto userDto);
        void Delete(int id);
    }
}