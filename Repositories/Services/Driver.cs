using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
namespace MASAR.Repositories.Services
{
    public class Driver : IDriver
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Driver(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public Task<List<string>> ViewAnnouncements()
        {
            var allAnnoun = _context.Announcement.Where(a => a.Audience == "Driver")
                .Select(a => "Title: " + a.Title + "   Content: " + a.Content + "   Created Time: " + a.CreatedTime).ToListAsync();
            return allAnnoun;
        }
        public async Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo)
        {
            var driverById = await _userManager.Users.Where(e => e.Id == driveInfo.DriverId).FirstOrDefaultAsync();

            if (driverById.Id == driveInfo.DriverId)
            {
                var result = new DriverProfile
                {
                    DriverProfileId = driveInfo.DriverProfileId,
                    DriverId = driverById.Id,
                    LicenseNumber = driveInfo.LicenseNumber,
                    BusId = driveInfo.BusId,
                    Status = driveInfo.Status
                };
                var resultWithPass = _context.DriverProfile.Add(result);
                await _context.SaveChangesAsync();
                return result;
            }
            return null;
        }
        public async Task<DriverProfile> UpdateDriver(string email, DriverProfile driver)
        {
            try
            {
                var driverById = await _userManager.FindByIdAsync(driver.DriverId.ToString());

                if (email == driverById.Email)
                {
                    _context.Entry(driver).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return driver;
                }
                return null;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO)
        {
            var driverById = await _context.DriverProfile.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var driver = await _userManager.FindByIdAsync(driverById.DriverId);
            if (driver.Email == email && maintenanceDTO.BusId == driverById.BusId)
            {
                var maintenance = new Maintenance
                {
                    MaintenanceId = maintenanceDTO.MaintenanceId,
                    DriverId = driverById.DriverProfileId,
                    BusId = driverById.BusId,
                    RequestDate = maintenanceDTO.RequestDate,
                    Description = maintenanceDTO.Description,
                    Status = maintenanceDTO.Status
                };
                _context.Maintenance.Add(maintenance);
                await _context.SaveChangesAsync();
                return maintenance;
            }
            else
            {
                return null;
            }
        }
    }
}