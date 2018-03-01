using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SensorService.Shared.Dtos;
using SensorService.UI.Managers;
using SensorService.UI.Models;

namespace SensorService.UI.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly ISessionManager _sessionManager;

        public MenuViewComponent(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var userDto = new UserDto
            {
                UserName = _sessionManager.UserName,
                Email = _sessionManager.Email,
                IsAdministrator = _sessionManager.IsAdministrator
            };

            var menuModel = new MenuModel {User = userDto, IsAuthenticated = _sessionManager.IsAuthenticated};

            return View(menuModel);
        }

        public string Name => _sessionManager.UserName;
        public string Email => _sessionManager.Email;
        public bool IsAdministrator => _sessionManager.IsAdministrator;
    }
}
