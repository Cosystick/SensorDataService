using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SensorService.UI.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public string UserName
        {
            get
            {
                var identity = (ClaimsIdentity)User.Identity;
                return identity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
            }
        }
    }
}
