using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Users.Models;
using System.Threading.Tasks;

namespace Users.Infrastructure
{
    public class CustomUserValidator:UserValidator<AppUser>
    {
        public CustomUserValidator(AppUserManager mgr):base(mgr){}

        public override async Task<IdentityResult> ValidateAsync(AppUser item)
        {
            IdentityResult result = await base.ValidateAsync(item);
            if(!item.Email.ToLower().EndsWith("@example.com"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Only example.com email addresses are allowed");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}