using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;

namespace Users.Infrastructure
{
    public class CustomPasswordValidator:PasswordValidator
    {
        public override async Task<IdentityResult> ValidateAsync(string item)
        {
            IdentityResult result = await base.ValidateAsync(item);
            if (item.Contains("1234"))
            {
                var errors = result.Errors.ToList();
                errors.Add("Password cannot contain numeric sequences");
                result = new IdentityResult(errors);
            }
            return result;
        }
    }
}