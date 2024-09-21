using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using MASAR.Data;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusesController : ControllerBase
    {
        private readonly IBus _context;
        private readonly MASARDBContext _buslocation;

        public BusesController(IBus context)
        {
            _context = context;
        }
        // POST: api/Buses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "RequireAdminRole")]
        [HttpPost("CreateBusByAdmin")]
        public async Task<ActionResult<Bus>> PostBus(BusDTO busDTO)
        {
            return await _context.CreateBusInfo(busDTO);
        }
        private static Dictionary<string, BusLocation> _busLocations = new Dictionary<string, BusLocation>();
        [HttpPost("updateBusLocation")]
        public IActionResult UpdateLocation(BusLocationDTO location)
        {
            if (location == null)
            {
                return BadRequest("Invalid location data.");
            }
            var previousLocation = _busLocations.ContainsKey(location.BusId)
                                       ? _busLocations[location.BusId]
                                       : null; 
            var busLocation = new BusLocation
            {
                BusId=location.BusId,
                BusLocationId= location.BusLocationId,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                PreviuseLatitude= previousLocation.Latitude,
                PreviuseLongitude= previousLocation.Longitude,
                Timestamp = location.Timestamp
            };
            // Store or update the bus's location
            _busLocations[location.BusId] = busLocation;
            return Ok(new { message = "Location updated successfully." });
        }
        [HttpGet("getBusLocation")]
        public IActionResult GetBusLocation(string busId)
        {
            if (_busLocations.ContainsKey(busId))
            {
                return Ok(_busLocations[busId]);
            }
            return NotFound("Bus location not found.");
        }      
    }
}