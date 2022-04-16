using Microsoft.Owin;
using Owin;
using School_MIS.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System;

[assembly: OwinStartupAttribute(typeof(School_MIS.Startup))]
namespace School_MIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRoleAndUsers();
        }

        public void createRoleAndUsers()
        {
            var context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            var user = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@manoj.com",
                BirthDate = DateTime.Now
            };

            var password = "pass123";

            var usr = userManager.Create(user, password);

            if (usr.Succeeded)
            {
                var result = userManager.AddToRole(user.Id, "Admin");
            }
            if (!roleManager.RoleExists("admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

               
            }

            if (!roleManager.RoleExists("Tutor"))
            {
                var role = new IdentityRole();
                role.Name = "Tutor";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("Cordinator"))
            {
                var role = new IdentityRole();
                role.Name = "Cordinator";
                roleManager.Create(role);
            }
        }
    }
}
