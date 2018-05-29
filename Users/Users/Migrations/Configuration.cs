namespace Users.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Users.Infrastructure;
    using Users.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<Users.Infrastructure.AppIdentityDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "Users.Infrastructure.AppIdentityDbContext";
        }

        protected override void Seed(Users.Infrastructure.AppIdentityDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //


            AppUserManager userMrg = new AppUserManager(new UserStore<AppUser>(context));
            AppRoleManager roleMrg = new AppRoleManager(new RoleStore<AppRole>(context));

            String roleName = "SuperAdministors";
            String userName = "SuperAdmin";
            String password = "SuperAmin12";
            String email = "SuperAdmin@exmaple.com";

            if (!roleMrg.RoleExists(roleName))
                roleMrg.Create(new AppRole(roleName));

            AppUser user = userMrg.FindByName(userName);
            if (user == null)
            {
                userMrg.Create(new AppUser { UserName = userName, Email = email }, password);
                user = userMrg.FindByName(userName);
            }

            if (!userMrg.IsInRole(user.Id, roleName))
                userMrg.AddToRole(user.Id, roleName);

            foreach (AppUser dbUser in userMrg.Users)
            {
                dbUser.City = Cities.PARIS;
            }
            context.SaveChanges();
        }

    }
}
