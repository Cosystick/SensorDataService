using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.UI.Managers;

namespace SensorService.UI.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public LogoutModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
        }

        public IActionResult OnGet()
        {
            _apiManager.DoLogout();
            return new RedirectToPageResult("/Login");
        }
    }
}