using Dashboard.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dashboard.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
           
           _roleManager = roleManager;
        }
        public  async Task<IActionResult> Index()
        {
            var Roles = await _roleManager.Roles.ToListAsync();
            return View(Roles);
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExsits = await _roleManager.RoleExistsAsync(model.Name);
                if (!RoleExsits)
                {
                    await _roleManager.CreateAsync(new IdentityRole(model.Name.Trim()));
                    return RedirectToAction(nameof(Index), await _roleManager.Roles.ToListAsync());
                }
                else
                {
                    ModelState.AddModelError("Name", "Role Is Exists");
                    return View(nameof(Index), await _roleManager.Roles.ToListAsync());


                }
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            await _roleManager.DeleteAsync(role);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            var mappedRole = new RoleViewModel()
            {
                Name = role.Name
            };
            return View(mappedRole);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(string id, RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var RoleExsits = await _roleManager.RoleExistsAsync(model.Name);
                if (!RoleExsits)
                {
                    var role = await _roleManager.FindByIdAsync(model.Id);
                    role.Name = model.Name;
                    await _roleManager.UpdateAsync(role);
                    return RedirectToAction(nameof(Index));
                }

                else
                {
                    ModelState.AddModelError("Name", "Role Is Exists");
                    return View(nameof(Index), await _roleManager.Roles.ToListAsync());


                }
            }
            return RedirectToAction(nameof(Index));

        }

    }
}
