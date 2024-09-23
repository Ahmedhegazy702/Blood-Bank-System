using BloodBank.Core.Entites.Identity;
using BloodBankSystem.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    public class AdminController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;

        public AdminController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto login)
        {
            var user = await userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Invalid Email");
                return RedirectToAction(nameof(Login));
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, login.Password, false);
            if (!result.Succeeded && !await userManager.IsInRoleAsync(user, "Admin"))
            {
                ModelState.AddModelError(string.Empty, "you are not authorized");
                return RedirectToAction(nameof(Login));
            }
            else
                return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
