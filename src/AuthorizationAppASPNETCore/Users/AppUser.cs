using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AuthorizationAppASPNETCore.Users
{
    public enum Cities
    {
        London, Paris, Chicago
    }

    public class AppUser : IdentityUser
    {
        public Cities City { get; set; }
    }
}
