using BloodBank.Core.Entites.Identity;
using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
          _userManager = userManager;
          _roleManager = roleManager;
        }
        public async Task<IActionResult> Index()
        {
            var Users = await _userManager.Users.Select(u => new UserViewModel
            {
                Id = u.Id,
                UserName = u.UserName,
                Email = u.Email,
                DisplayName = u.DisplayName,
                PhoneNumber = u.PhoneNumber,
                BloodType = u.BloodType,
                Roles = _userManager.GetRolesAsync(u).Result
            }).ToListAsync();

            return View(Users);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var AllRoles = await _roleManager.Roles.ToListAsync();
            var viewModel = new UserRolesViewModel()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Roles = AllRoles.Select(r => new RoleViewModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    isSelected = _userManager.IsInRoleAsync(user, r.Name).Result
                }).ToList(),
            };

            return View(viewModel);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(UserRolesViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            var userRole = await _userManager.GetRolesAsync(user);
            foreach (var role in model.Roles)
            {
                if (userRole.Any(r => r == role.Name) && !role.isSelected)
                {
                    await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                if (!userRole.Any(r => r == role.Name) && role.isSelected)
                {
                    await _userManager.AddToRoleAsync(user, role.Name);
                }

            }

            return RedirectToAction(nameof(Index));
        }


    }
}
