﻿using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Users.Models
{
    public class AppRole:IdentityRole
    {
        public AppRole() : base() { }

        public AppRole(String name) : base(name) { }
    }
}