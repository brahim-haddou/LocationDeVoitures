using System;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace LocationDeVoitures.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Administrateur> Administrateurs { get; set; }
        public virtual DbSet<Agence> Agences { get; set; }
        public virtual DbSet<Locataire> Locataires { get; set; }
        public virtual DbSet<Voiture> Voitures { get; set; }
        public virtual DbSet<ImagesVoiture> ImagesVoitures { get; set; }
        public virtual DbSet<Offre> Offres { get; set; }
        public virtual DbSet<Location> Locations { get; set; }
        public virtual DbSet<Probleme> Problemes { get; set; }
        public virtual DbSet<ListFavoris> ListFavoris { get; set; }
        public virtual DbSet<ListNoire> ListNoire { get; set; }
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        internal void Refresh(object clientWins, object articles)
        {
            throw new NotImplementedException();
        }
    }
}