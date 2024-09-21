using Microsoft.AspNetCore.Identity;

namespace MASAR.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }
        public string Roles { get; set; }
        // Relationships
        public ICollection<Schedule> Schedules { get; set; }
        public ICollection<Announcement> Announcements { get; set; }
        public DriverProfile DriverProfiles { get; set; }
    }
}
