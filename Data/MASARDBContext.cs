using MASAR.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MASAR.Repositories.Interfaces;
namespace MASAR.Data
{
    public class MASARDBContext : IdentityDbContext<ApplicationUser>
    {
        public MASARDBContext(DbContextOptions<MASARDBContext> options) : base(options)
        {
        }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Schedule> Schedule { get; set; }
        public DbSet<Routing> Route { get; set; }
        public DbSet<BusLocation> BusLocation { get; set; }
        public DbSet<Maintenance> Maintenance { get; set; }
        public DbSet<DriverProfile> DriverProfile { get; set; }
        public DbSet<Announcement> Announcement { get; set; }
        public DbSet<Bus> Bus { get; set; }
        //ERD Visualization
        //The entities will be linked as follows:
        //User(1) — (M)Schedule
        //User(1) — (M)Announcement
        //Schedule(M) — (1) Route
        //Maintenance(M) — (1) DriverProfile
        //Maintenance(M) — (1) Bus
        //DriverProfile(1) — (1) Bus
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // User -> Schedules
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Schedules)
                .WithOne(s => s.User)
                .HasForeignKey(s => s.ScheduleId)
                .OnDelete(DeleteBehavior.Restrict);
            // User -> Announcements
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.Announcements)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.AdminId)
                .OnDelete(DeleteBehavior.Restrict);
            // Schedule -> Route
            modelBuilder.Entity<Schedule>()
                .HasOne(s => s.Routing)
                .WithMany(r => r.Schedules)
                .HasForeignKey(s => s.RoutingId)
                .OnDelete(DeleteBehavior.Restrict);
            // Maintenance -> DriverProfile
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.DriverProfile)
                .WithMany(b => b.Maintenances)
                .HasForeignKey(m => m.DriverId)
                .OnDelete(DeleteBehavior.Restrict);
            // Maintenance -> Bus
            modelBuilder.Entity<Maintenance>()
                .HasOne(m => m.Bus)
                .WithMany(b => b.Maintenances)
                .HasForeignKey(m => m.BusId)
                .OnDelete(DeleteBehavior.Restrict);
            // DriverProfile -> Bus (One-to-One)
            modelBuilder.Entity<DriverProfile>()
                .HasOne(dp => dp.Bus) // Navigation property for Bus
                .WithOne(b => b.DriverProfiles) // Corresponding navigation property in Bus
                .HasForeignKey<DriverProfile>(dp => dp.BusId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Restrict);
            // DriverProfile -> User (One-to-One)
            modelBuilder.Entity<DriverProfile>()
                .HasOne(dp => dp.User) // Navigation property for User
                .WithOne(u => u.DriverProfiles) // Corresponding navigation property in User
                .HasForeignKey<DriverProfile>(dp => dp.DriverId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Restrict);
            // DriverProfile -> User (One-to-One)
            modelBuilder.Entity<BusLocation>()
                .HasOne(dp => dp.Bus) // Navigation property for User
                .WithOne(u => u.BusLocations) // Corresponding navigation property in User
                .HasForeignKey<BusLocation>(dp => dp.BusId) // Foreign key in DriverProfile
                .OnDelete(DeleteBehavior.Restrict);
            // Seeding Admin data
            var hasher = new PasswordHasher<ApplicationUser>();
            // Create a user instance
            var user = new ApplicationUser
            {
                Id = "200",
                UserName = "Admin",
                Email = "Admin@gmail.com",
                PhoneNumber = "19283746",
                UserType = "Admin",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@GMAIL.COM",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                Roles = "Admin",
                SecurityStamp = Guid.NewGuid().ToString("D"),
                ConcurrencyStamp = Guid.NewGuid().ToString("D"),
            };
            // Hash the password and assign it
            user.PasswordHash = hasher.HashPassword(user, "AdminStrongPassword123");
            // Seed the user
            modelBuilder.Entity<ApplicationUser>().HasData(user);
            // Adding Roles 
            seedRoles(modelBuilder, "Admin", "create", "update", "delete","read");
            seedRoles(modelBuilder, "Driver", "update", "read");
            seedRoles(modelBuilder, "Student", "update", "read");
            seedRoles(modelBuilder, "User", "update" ,"read");
        }
        private void seedRoles(ModelBuilder modelBuilder, string roleName, params string[] permissions)
        {
            var role = new IdentityRole
            {
                Id = roleName.ToLower(),
                Name = roleName,
                NormalizedName = roleName.ToUpper(),
                ConcurrencyStamp = Guid.NewGuid().ToString()  // Unique Concurrency Stamp
            };
            // Add claims for the role
            var claims = permissions.Select(permission => new IdentityRoleClaim<string>
            {
                Id = Guid.NewGuid().GetHashCode(), // Unique identifier
                RoleId = role.Id,
                ClaimType = "permission",
                ClaimValue = permission
            }).ToArray();
            // Seed the role and its claims
            modelBuilder.Entity<IdentityRole>().HasData(role);
            modelBuilder.Entity<IdentityRoleClaim<string>>().HasData(claims);
        }
    }
}