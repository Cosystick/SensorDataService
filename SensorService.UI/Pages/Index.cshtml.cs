using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.UI.Managers;

namespace SensorService.UI.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ISessionManager _sessionManager;

        public IndexModel(ISessionManager sessionManager)
        {
            _sessionManager = sessionManager;
        }
        public void OnGet()
        {

        }

        public string UserName => _sessionManager.UserName;
        public bool IsAdministrator => _sessionManager.IsAdministrator;
    }
}
