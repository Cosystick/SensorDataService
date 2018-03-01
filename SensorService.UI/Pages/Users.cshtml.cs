using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.Shared.Dtos;
using SensorService.UI.Managers;

namespace SensorService.UI.Pages
{
    public class UsersModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public UsersModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(string action)
        {
            return new RedirectToPageResult("/UserInfo");
        }

        public List<UserDto> Users => _apiManager.GetUsers().Result;
    }
}