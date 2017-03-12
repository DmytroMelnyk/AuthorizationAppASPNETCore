using AuthorizationAppASPNETCore.Users;
using AuthorizationAppASPNETCore.Users.Infrastructure;
using AuthorizationAppASPNETCore.Users.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthorizationAppASPNETCore.Extensions
{
    public static class AppIdentityDbContextExtensions
    {
        public static void EnsureSeedData(this AppIdentityDbContext context)
        {
            AppUserManager userMgr = new AppUserManager(new UserStore<AppUser, AppRole, AppIdentityDbContext, string>(context), null, new PasswordHasher<AppUser>(), null, null, null, null, null, null);
            AppRoleManager roleMgr = new AppRoleManager(new RoleStore<AppRole, AppIdentityDbContext, string>(context), null, null, null, null, null);
            string roleName = "Administrators";
            string userName = "Admin";
            string password = "MySecret";
            string email = "admin@example.com";

            if (!roleMgr.RoleExistsAsync(roleName).Result)
            {
                roleMgr.CreateAsync(new AppRole(roleName)).Wait();
            }

            AppUser user = userMgr.FindByNameAsync(userName).Result;
            if (user == null)
            {
                user = new AppUser { UserName = userName, Email = email };
                userMgr.CreateAsync(user, password).Wait();
                user = userMgr.FindByNameAsync(userName).Result;
            }

            if (!userMgr.IsInRoleAsync(user, roleName).Result)
            {
                userMgr.AddToRoleAsync(user, roleName).Wait();
            }

            context.SaveChanges();
        }
    }
}
