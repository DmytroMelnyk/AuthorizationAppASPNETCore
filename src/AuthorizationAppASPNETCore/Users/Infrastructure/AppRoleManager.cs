using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using AuthorizationAppASPNETCore.Users.Models;

namespace AuthorizationAppASPNETCore.Users.Infrastructure
{
    public class AppRoleManager : RoleManager<AppRole>
    {
        public AppRoleManager(
            IRoleStore<AppRole> store, 
            IEnumerable<IRoleValidator<AppRole>> roleValidators, 
            ILookupNormalizer keyNormalizer, 
            IdentityErrorDescriber errors, 
            ILogger<RoleManager<AppRole>> logger, 
            IHttpContextAccessor contextAccessor) : 
                base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }
    }
}
