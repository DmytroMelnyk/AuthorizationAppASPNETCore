using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthorizationAppASPNETCore.Users.Models
{
    public class AppRole : IdentityRole
    {
        public AppRole()
        {
        }

        public AppRole(string name) : base(name)
        {
        }
    }
}
