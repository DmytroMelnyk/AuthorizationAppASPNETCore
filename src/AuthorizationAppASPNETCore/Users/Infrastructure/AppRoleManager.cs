using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuthorizationAppASPNETCore.Users.Infrastructure
{
    public class AppRoleManager : RoleManager<AppUser>
    {
        public AppRoleManager(
            IRoleStore<AppUser> store, 
            IEnumerable<IRoleValidator<AppUser>> roleValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            ILogger<RoleManager<AppUser>> logger, 
            IHttpContextAccessor contextAccessor) : 
                base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }
    }
}
