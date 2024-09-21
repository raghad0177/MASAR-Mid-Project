using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IDriver 
    {
        public Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo);
        public Task<DriverProfile> UpdateDriver(string email, DriverProfile driver);
        public Task<List<string>> ViewAnnouncements();
        public Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO);
    }
}