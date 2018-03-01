using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SensorService.Shared.Dtos;
using SensorService.UI.Managers;
using SensorService.UI.Validations;

namespace SensorService.UI.Pages
{
    public class UserInfoModel : PageModel, IPasswordModel
    {
        private readonly IApiManager _apiManager;
        private readonly ISessionManager _sessionManager;

        public UserInfoModel(IApiManager apiManager, ISessionManager sessionManager)
        {
            _apiManager = apiManager;
            _sessionManager = sessionManager;
        }
        public async Task OnGet(int id)
        {
            if (id > 0)
            {
                var userDto = await _apiManager.GetUserById(id);
                Id = userDto.Id;
                UserName = userDto.UserName;
                Email = userDto.Email;
                IsAdministrator = userDto.IsAdministrator;
            }
        }

        [HttpPost]
        public async Task<IActionResult> OnPostAsync()
        {
            if (!string.IsNullOrEmpty(Password))
            {
                if (Password != PasswordCheck)
                {
                    ModelState.AddModelError("Password", "Passwords does not match");
                }

                if (Password.Length < 6)
                {
                    ModelState.AddModelError("Password", "Password needs to be at least 5 chars.");
                }
            }
            else
            {
                if (Id == 0)
                {
                    ModelState.AddModelError("Password", "You need to set a password");
                }
            }

            if (!IsValidEmail(Email))
            {
                ModelState.AddModelError("Email", "Not a valid e-mail address.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            UserDto userDto = null;
            if (Id > 0)
            {
                userDto = await _apiManager.GetUserById(Id);
            }
            else
            {
                userDto = new UserDto();
            }

            userDto.UserName = UserName;
            userDto.Email = Email;
            if (!string.IsNullOrEmpty(Password))
            {
                userDto.Password = Password;
            }

            if (_sessionManager.IsAdministrator)
            {
                userDto.IsAdministrator = IsAdministrator;
            }


            if (userDto.Id == 0)
            {
                var result = await _apiManager.CreateUser(userDto);
                return new RedirectToPageResult("/UserInfo", new { result.Id });

            }
            await _apiManager.UpdateUser(userDto);
            return new RedirectToPageResult("/UserInfo", new { userDto.Id });
        }

        private bool IsValidEmail(string email)
        {
            var emailAttribute = new EmailAddressAttribute();
            return emailAttribute.IsValid(email);
        }

        [BindProperty]
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        [BindProperty]
        [DisplayName("User name")]
        [StringLength(60, MinimumLength = 3)]
        [Required(AllowEmptyStrings = false, ErrorMessage = "Username has to be at least 3 characters")]
        public string UserName { get; set; }

        [BindProperty]
        [EmailAddress]
        [DisplayName("E-mail")]
        [StringLength(60, MinimumLength = 7, ErrorMessage = "Needs to be a valid e-mail address")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [BindProperty]
        [DataType(DataType.Password)]
        [PasswordCheck]
        [StringLength(60, MinimumLength = 5)]
        public string Password { get; set; }

        [BindProperty]
        [DisplayName("Verify password")]
        [DataType(DataType.Password)]
        [StringLength(60, MinimumLength = 5)]
        public string PasswordCheck { get; set; }

        [BindProperty]
        [DisplayName("Admin")]
        public bool IsAdministrator { get; set; }

    }
}