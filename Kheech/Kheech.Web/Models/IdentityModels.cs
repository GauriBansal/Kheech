using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Kheech.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public const string DisplayNameClaimType = "DisplayName";

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            userIdentity.AddClaim(new Claim(ClaimTypes.GivenName, FirstName));
            return userIdentity;
        }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public virtual ICollection<KheechEvent> KheechEvents { get; set; }

        public virtual ICollection<Friendship> FriendshipsStarted { get; set; }
        public virtual ICollection<Friendship> FriendshipsJoined { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<KheechEvent> KheechEvents { get; set; }
        public DbSet<KheechUser> KheechUsers { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendshipStatus> FriendshipStatuses { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<GroupUser> GroupUsers { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Moment> Moments { get; set; }
        public DbSet<KheechComment> KheechComments { get; set; }
        public DbSet<InviteFriend> InviteFriends { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<Friendship>(u => u.FriendshipsStarted)
                .WithRequired()


                .HasForeignKey(f => f.RecipientId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany<Friendship>(u => u.FriendshipsJoined)
                .WithRequired()
                .HasForeignKey(f => f.InitiatorId)
                .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}