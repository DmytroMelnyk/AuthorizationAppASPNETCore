using AuthorizationAppASPNETCore.Users;
using AuthorizationAppASPNETCore.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Users.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppUserManager _userManager;
        private readonly IUserValidator<AppUser> _userValidator;
        private readonly IPasswordValidator<AppUser> _passwordValidator;

        public AdminController(AppUserManager userManager, IUserValidator<AppUser> userValidator, IPasswordValidator<AppUser> passwordValidator)
        {
            _userManager = userManager;
            _userValidator = userValidator;
            _passwordValidator = passwordValidator;
        }

        public ActionResult Index()
        {
            return View(_userManager.Users.AsEnumerable());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Create(CreateModel model)
        {
            if (ModelState.IsValid)
            {
                AppUser user = new AppUser { UserName = model.Name, Email = model.EMail };
                IdentityResult result = await _userManager.CreateAsync(user,
                model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                AddErrorsFromResult(result);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Delete(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }

                return View("Error", result.Errors);
            }

            return View("Error", new string[] { "User Not Found" });
        }

        public async Task<ActionResult> Edit(string id)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                return View(user);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Edit(string id, string email, string password)
        {
            AppUser user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.Email = email;
                IdentityResult validEmail = await _userValidator.ValidateAsync(_userManager, user);
                if (!validEmail.Succeeded)
                {
                    AddErrorsFromResult(validEmail);
                }
                
                IdentityResult validPass = null;
                if (password != string.Empty)
                {
                    validPass = await _passwordValidator.ValidateAsync(_userManager, user, password);
                    if (validPass.Succeeded)
                    {
                        await _userManager.RemovePasswordAsync(user);
                        await _userManager.AddPasswordAsync(user, password);
                    }
                    else
                    {
                        AddErrorsFromResult(validPass);
                    }
                }


                if ((validEmail.Succeeded && validPass == null) || (validEmail.Succeeded && password != string.Empty && validPass.Succeeded))
                {
                    IdentityResult result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }

                    AddErrorsFromResult(result);
                }
            }
            else
            {
                ModelState.AddModelError("", "User Not Found");
            }
            return View(user);
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