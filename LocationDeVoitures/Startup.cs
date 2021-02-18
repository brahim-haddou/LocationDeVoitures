using LocationDeVoitures.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using System.Linq;

[assembly: OwinStartupAttribute(typeof(LocationDeVoitures.Startup))]
namespace LocationDeVoitures
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            PopulateUserAndRoles();
        }

        public void PopulateUserAndRoles()
        {
            var db = new ApplicationDbContext();

            if (!db.Roles.Any(x => x.Name == MesConstants.RoleAdministrateur))
            {
                db.Roles.Add(new IdentityRole() { Name = MesConstants.RoleAdministrateur });
                db.SaveChanges();
            }
            if (!db.Roles.Any(x => x.Name == MesConstants.RoleAgence))
            {
                db.Roles.Add(new IdentityRole() { Name = MesConstants.RoleAgence });
                db.SaveChanges();
            }
            if (!db.Roles.Any(x => x.Name == MesConstants.RoleLocataire))
            {
                db.Roles.Add(new IdentityRole() { Name = MesConstants.RoleLocataire });
                db.SaveChanges();
            }


            if (!db.Users.Any(x => x.UserName == MesConstants.RoleAdministrateur))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var newUser = new ApplicationUser
                {
                    Email = MesConstants.RoleAdministrateur + "@gmail.com",
                    UserName = MesConstants.RoleAdministrateur
                };
                userManager.Create(newUser, MesConstants.RoleAdministrateur);
                userManager.AddToRole(newUser.Id, MesConstants.RoleAdministrateur);
                db.SaveChanges();
            }
            if (!db.Users.Any(x => x.UserName == MesConstants.RoleAgence))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var newUser = new ApplicationUser
                {
                    Email = MesConstants.RoleAgence + "@gmail.com",
                    UserName = MesConstants.RoleAgence
                };
                userManager.Create(newUser, MesConstants.RoleAgence);
                userManager.AddToRole(newUser.Id, MesConstants.RoleAgence);
                db.SaveChanges();
            }
            if (!db.Users.Any(x => x.UserName == MesConstants.RoleLocataire))
            {
                var userStore = new UserStore<ApplicationUser>(db);
                var userManager = new ApplicationUserManager(userStore);

                var roleStore = new RoleStore<IdentityRole>(db);
                var roleManager = new RoleManager<IdentityRole>(roleStore);

                var newUser = new ApplicationUser
                {
                    Email = MesConstants.RoleLocataire + "@gmail.com",
                    UserName = MesConstants.RoleLocataire
                };
                userManager.Create(newUser, MesConstants.RoleLocataire);
                userManager.AddToRole(newUser.Id, MesConstants.RoleLocataire);
                db.SaveChanges();
            }
        }
    }
}
