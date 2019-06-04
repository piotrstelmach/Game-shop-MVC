using GameShop.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(GameShop.Startup))]
namespace GameShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //var admin = new ApplicationUser { Email = "admin@gmail.com" };
            //var manager = new ApplicationUser { Email = "manager@gmail.com" };

            //var userman = UserManager.Create(admin, "DonVito3@");
            //var userman1 = UserManager.Create(manager, "DonVito2!");

            //if (userman.Succeeded)
            //{
            //    var result1 = UserManager.AddToRole(admin.Id, "Admin");

            //}

            //if (userman1.Succeeded)
            //{
            //    var result2 = UserManager.AddToRole(manager.Id, "Manager");

            //}


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {
                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

            }

            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);

            }

           


            //UserManager.AddToRole(admin, "Admin");
            // UserManager.AddToRole(manager.Id, "Manager");
        }
    }
}
