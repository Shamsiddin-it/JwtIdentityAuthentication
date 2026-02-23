using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RazorSide.Pages.Users
{
    public class UsersModel : PageModel
    {
        public List<ApplicationUser> users=new();
        private readonly UserManager<ApplicationUser> _userManager;
        public UsersModel(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }
        public void OnGet()
        {
            users = _userManager.Users.ToList();
        }
    }
}
