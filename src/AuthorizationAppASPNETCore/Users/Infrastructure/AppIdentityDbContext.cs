using AuthorizationAppASPNETCore.Users.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthorizationAppASPNETCore.Users.Infrastructure
{
    public class AppIdentityDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public AppIdentityDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
