using FrameworkBaseData.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebFrameworkBase.Models
{
    public class ApplicationUser : IdentityUser<int>
    {

        public ApplicationUser()
        {
            UserSettings = new List<UserSetting>();
        }

        public string UserPassword { get; set; }

        public string UserRoleName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string UserLanguageCode { get; set; }

        public List<UserSetting> UserSettings { get; set; }

        //public async Task<ApplicationUser> FindByNameAsync(UserManager<ApplicationUser> manager, ClaimsPrincipal principal)
        //{
        //    // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
        //    var userIdentity = await manager.FindByNameAsync(manager.GetUserName(principal));
        //    // Add custom user claims here.
        //    return userIdentity;
        //}
    }
}