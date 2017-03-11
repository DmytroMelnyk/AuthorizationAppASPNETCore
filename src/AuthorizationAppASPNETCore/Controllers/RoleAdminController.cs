using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AuthorizationAppASPNETCore.Users;
using AuthorizationAppASPNETCore.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace AuthorizationAppASPNETCore.Controllers
{
    public class RoleAdminController : Controller
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RoleAdminController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public ActionResult Index()
        {
            return View((object)Tuple.Create(_roleManager.Roles, _userManager));
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create([Required]string name)
        {
            if (!ModelState.IsValid)
            {
                return View(name);
            }

            var result = await _roleManager.CreateAsync(new AppRole(name));
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            AddErrorsFromResult(result);
            return View(name);
        }
        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                return View("Error", new[] { "Role Not Found" });
            }

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            return View("Error", result.Errors);
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppRole role = await _roleManager.FindByIdAsync(id);
            string[] memberIDs = role.Users.Select(x => x.UserId).ToArray();
            IEnumerable<AppUser> members = _userManager.Users.Where(x => memberIDs.Any(y => y == x.Id));
            IEnumerable<AppUser> nonMembers = _userManager.Users.Except(members);
            return View(new RoleEditModel
            {
                Role = role,
                Members = members,
                NonMembers = nonMembers
            });
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RoleModificationModel model)
        {
            IdentityResult result;
            if (!ModelState.IsValid)
            {
                return View("Error", new[] {"Role Not Found"});
            }

            foreach (var userId in model.IdsToAdd ?? Enumerable.Empty<string>())
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.AddToRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
            }

            foreach (var userId in model.IdsToDelete ?? Enumerable.Empty<string>())
            {
                var user = await _userManager.FindByIdAsync(userId);
                result = await _userManager.RemoveFromRoleAsync(user, model.RoleName);
                if (!result.Succeeded)
                {
                    return View("Error", result.Errors);
                }
            }

            return RedirectToAction("Index");
        }

        private void AddErrorsFromResult(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }
        }
    }
}
