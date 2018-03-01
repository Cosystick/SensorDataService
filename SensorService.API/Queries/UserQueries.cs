using System.Collections.Generic;
using System.Linq;
using SensorService.API.Models;
using SensorService.Shared.Dtos;

namespace SensorService.API.Queries
{
    internal class UserQueries : IUserQueries
    {
        private readonly SensorContext _context;

        public UserQueries(SensorContext context)
        {
            _context = context;
        }

        public User Login(string userName, string password)
        {
            return _context.Users.SingleOrDefault(u => u.UserName.ToLower() == userName.ToLower()
                                                       && u.Password == password);
        }

        public User GetById(int id)
        {
            return _context.Users.SingleOrDefault(u => u.Id == id);
        }

        public bool IsAdministrator(int id)
        {
            return _context.Users.Any(u => u.Id == id && u.IsAdministrator);
        }

        public List<User> Get()
        {
            return _context.Users.ToList();
        }

        public User Update(UserDto userDTO)
        {
            var user = _context.Users.SingleOrDefault(u => u.Id == userDTO.Id);
            if (user == null || 
                _context.Users.Any(u => u.UserName.ToLower() == userDTO.UserName.ToLower() && u.Id != userDTO.Id))
            {
                // If user doesnt exist or user name is already taken
                return null;
            }

            user.UserName = userDTO.UserName;
            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                user.Password = userDTO.Password;
            }
            user.Email = userDTO.Email;
            user.IsAdministrator = userDTO.IsAdministrator;
            _context.Users.Update(user);
            _context.SaveChanges();
            return user;

        }

        public User Insert(UserDto userDTO)
        {
            if (_context.Users.Any(u => u.UserName.ToLower() == userDTO.UserName.ToLower()))
            {
                return null;
            }

            var user = new User
            {
                UserName = userDTO.UserName,
                Email = userDTO.Email,
                IsAdministrator = userDTO.IsAdministrator,
                Password = userDTO.Password
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }

        public void Delete(int id)
        {
            var existingUser = _context.Users.SingleOrDefault(u => u.Id == id);
            if (existingUser == null)
            {
                return;
            }

            var userDevices = _context.Devices.Where(d => d.UserId == id).ToList();
            _context.RemoveRange(userDevices);
            _context.Remove(existingUser);
            _context.SaveChanges();
        }

    }
}
