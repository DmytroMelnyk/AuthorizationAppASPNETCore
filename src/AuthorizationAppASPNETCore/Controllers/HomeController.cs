using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AuthorizationAppASPNETCore.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAppASPNETCore.Controllers
{
    public class HomeController : Controller
    {
        private readonly UserManager<AppUser> _manager;

        public HomeController(UserManager<AppUser> manager)
        {
            _manager = manager;
        }

        [Authorize(Roles = "Users")]
        public IActionResult Index()
        {
            return View(GetData());
        }

        [Authorize]
        public IActionResult OtherAction()
        {
            return View("Index", GetData());
        }

        private Dictionary<string, object> GetData([CallerMemberName]string actionName = null)
        {
            return new Dictionary<string, object>
            {
                ["Action"] = actionName,
                ["User"] = HttpContext.User.Identity.Name,
                ["Authenticated"] = HttpContext.User.Identity.IsAuthenticated,
                ["Auth type"] = HttpContext.User.Identity.AuthenticationType,
                ["In Users Role"] = HttpContext.User.IsInRole("User")
            };
        }

        [Authorize]
        public async Task<IActionResult> UserProps()
        {
            var user = await _manager.FindByNameAsync(HttpContext.User.Identity.Name);
            return View(user);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> UserProps(Cities city)
        {
            var user = await _manager.FindByNameAsync(HttpContext.User.Identity.Name);
            user.City = city;
            await _manager.UpdateAsync(user);
            return View(user);
        }
    }
}
