using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Users.Infrastructure
{

    public class ClaimsAccessAttribute:AuthorizeAttribute
    {
        public String Issuer { get; set; }
        public String ClaimType { get; set; }
        public String Value { get; set; }

        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return httpContext.User.Identity.IsAuthenticated &&
                httpContext.User.Identity is ClaimsIdentity &&
                ((ClaimsIdentity)httpContext.User.Identity).HasClaim(x => x.Issuer == Issuer && x.Type == ClaimType && x.Value == Value);
        }
    }
}