using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthorizationAppASPNETCore.Users.Models
{
    public class AppRole : IdentityRole<string>
    {
        public AppRole()
        {
        }

        public AppRole(string name) : base(name)
        {
        }
    }
}
