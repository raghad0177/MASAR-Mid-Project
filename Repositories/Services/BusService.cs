using MASAR.Data;
using MASAR.Model;
using MASAR.Repositories.Services;
using Microsoft.AspNetCore.Identity;
using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


namespace MASAR.Repositories.Services
{
    public class BusService : IBus
    {
        private readonly MASARDBContext _context;
        // inject jwt service
        public BusService(MASARDBContext context)
        {
            _context = context;
        }       
        public async Task<Bus> CreateBusInfo(BusDTO busDTO)
        {
            Bus bus = new Bus
            {
                BusId = busDTO.BusId,
                PlateNumber = busDTO.PlateNumber,
                LicenseExpiry = busDTO.LicenseExpiry,
                CapacityNumber = busDTO.CapacityNumber,
                Status = busDTO.Status,
                CurrentLocation = "LTUC",
                UpdatedTime = DateTime.Now
            };
             _context.Bus.Add(bus);
            await _context.SaveChangesAsync();
            return bus;
        }
    }
}