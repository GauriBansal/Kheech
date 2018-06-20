using Kheech.Web.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kheech.Web.Migrations
{
    using Microsoft.AspNet.Identity;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Kheech.Web.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Kheech.Web.Models.ApplicationDbContext context)
        {
            var roleStore = new RoleStore<IdentityRole>(context);
            var roleManager = new RoleManager<IdentityRole>(roleStore);
            var role = new IdentityRole { Name = "AppAdmin" };

            var userStore = new UserStore<IdentityUser>(context);
            var userManager = new UserManager<IdentityUser>(userStore);

            var phil = context.Users.FirstOrDefault(u => u.Email == "phil.scholtes@gmail.com");
            if (phil == null)
            {
                phil = new ApplicationUser
                {
                    FirstName = "Phil",
                    LastName = "Scholtes",
                    UserName = "phil.scholtes@gmail.com",
                    Email = "phil.scholtes@gmail.com"
                };

                var result = userManager.Create(phil, "Admin123!");
            }

            var chris = context.Users.FirstOrDefault(u => u.Email == "chris@gmail.com");
            if (chris == null)
            {
                chris = new ApplicationUser
                {
                    FirstName = "Chris",
                    LastName = "Scholtes",
                    UserName = "chris@gmail.com",
                    Email = "chris@gmail.com"
                };

                var result = userManager.Create(chris, "Admin123!");
            }

            phil.FriendshipsStarted.Add(new Friendship()
            {
                //FriendshipStatus = FriendshipStatuses.
            });


        }
    }
}
