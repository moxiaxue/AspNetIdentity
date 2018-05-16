using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Users.Models;

namespace Users.Infrastructure
{
    public class AppIdentityDbContext:IdentityDbContext<AppUser>
    {
        //connect with database
        public AppIdentityDbContext():base("IdentityDb"){}

        static AppIdentityDbContext()
        {
            Database.SetInitializer<AppIdentityDbContext>(new IdentityDbInit());
        }

        public static AppIdentityDbContext Create()
        {
            return new AppIdentityDbContext();
        }
    }

    //when the schema is first created through the Entity Framework Code First feature,this class will be called
    public class IdentityDbInit:DropCreateDatabaseIfModelChanges<AppIdentityDbContext>
    {
        protected override void Seed(AppIdentityDbContext context)
        {
            PerformInitialSetup(context);
            base.Seed(context);
        }

        public void PerformInitialSetup(AppIdentityDbContext context)
        {
            AppUserManager userMrg = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMrg = new AppRoleManager(new RoleStore<AppRole>(context));

            String roleName = "SuperAdministors";
            String userName = "SuperAdmin";
            String password = "SuperAmin12";
            String email = "SuperAdmin@exmaple.com";

            if (!roleMrg.RoleExists(roleName))
                roleMrg.Create(new AppRole(roleName));

            AppUser user = userMrg.FindByName(userName);
            if(user==null)
            {
                userMrg.Create(new AppUser { UserName = userName, Email = email }, password);
                user = userMrg.FindByName(userName);
            }

            if (!userMrg.IsInRole(user.Id, roleName))
                userMrg.AddToRole(user.Id, roleName);
        }
    }
}