using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Users.Models
{

    public enum Cities
    {
        LONDON,
        PARIS,
        CHICAGO
    }

    public class AppUser:IdentityUser
    {
        public Cities City { get; set; }
    }
}