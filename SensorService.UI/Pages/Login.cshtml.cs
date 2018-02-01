using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.UI.Managers;
using SensorService.UI.Models;

namespace SensorService.UI.Pages
{
    public class LoginPageModel : PageModel
    {
        private readonly IApiManager _apiManager;

        public LoginPageModel(IApiManager apiManager)
        {
            _apiManager = apiManager;
            Error = string.Empty;
        }
        public void OnGet()
        {
        }

        [BindProperty]
        public string UserName { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var loginModel = new LoginModel {UserName = UserName, Password = Password};

            try
            {
                if (!await _apiManager.DoLogin(loginModel))
                {
                    Error = "You don't exist, go away!";
                    return Page();
                }

            }
            catch (Exception ex)
            {
                Error = string.Format("An error occurred: {0}", ex.Message);
            }

            return RedirectToPage("/Devices");
        }

        public string Error { get; set; }
    }
}
