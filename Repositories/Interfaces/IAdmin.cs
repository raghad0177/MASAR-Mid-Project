using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IAdmin
    {
        public Task<ApplicationUser> GetUserByEmail(string email);
        public Task DeleteUser(string email);
        public Task<ApplicationUser> CreateUser(SignUpDTO user);
        public Task<List<ApplicationUser>> GetAllDrivers();
        public Task<List<ApplicationUser>> GetAllUsers();
        public Task<Announcement> CreateAnnouncement(string email, AnnouncementDTO AnnouncementDto);
        public Task<List<Maintenance>> GetAllMaintenanceRequests();
        public Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO);
    }
}