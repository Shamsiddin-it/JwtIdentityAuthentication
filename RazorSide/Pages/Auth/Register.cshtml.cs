using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using WebApi.DTOs;

namespace RazorSide.Pages.Auth
{
    public class RegisterModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public RegisterModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [BindProperty]
        public RegisterDto registerDto {get; set;} = new();
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var email = registerDto.Email.Trim();
            var fullName = registerDto.FullName.Trim();

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                }

                return Page();
            }

            return RedirectToPage("/Users/Users");
        }

    }
}
