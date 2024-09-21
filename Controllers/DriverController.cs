using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Data;
using MASAR.Model;
using Microsoft.AspNetCore.Authorization;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using System.IO;

namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        private readonly IDriver _context;
        public DriverController(IDriver context)
        {
            _context = context;
        }
        //GET : api/Driver
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<List<string>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncements();
        }
        //POST : api/Driver
        [HttpPost("CreateDriverInfo")]
        [Authorize (Policy = "RequireDriverRole")]
        public async Task<DriverProfile> CreateInfo(string email, DriverInfoDTO driveInfo)
        {
            return await _context.CreateInfo(email, driveInfo);
        }
        //POST : api/Driver
        [HttpPost("UpdateDriverProfile")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<DriverProfile> UpdateDriver(string email, DriverProfile driver)
        {
            return await _context.UpdateDriver(email, driver);
        }
        //POST : api/Driver
        [HttpPost("MaintenanceRequest")]
        [Authorize(Policy = "RequireDriverRole")]
        public async Task<Maintenance> MaintenanceRequest(string email, MaintenancaDTO maintenanceDTO)
        {
            return await _context.MaintenanceRequest(email, maintenanceDTO);
        }
        //// GET: api/Driver
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        //{
        //    if (_context.ApplicationUser == null)
        //    {
        //        return NotFound();
        //    }
        //    return await _context.ApplicationUser.ToListAsync();
        //}

        //// GET: api/Driver/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ApplicationUser>> GetUser(int id)
        //{
        //  if (_context.ApplicationUser == null)
        //  {
        //      return NotFound();
        //  }
        //    var user = await _context.ApplicationUser.FindAsync(id);

        //    if (user == null)
        //    {
        //        return NotFound();
        //    }

        //    return user;
        //}

        // PUT: api/Driver/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutUser(int id, ApplicationUser user)
        //{
        //    if (id != user.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(user).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!UserExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Driver
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //        [HttpPost]
        //        public async Task<ActionResult<User>> PostUser(User user)
        //        {
        //            if (_context.User == null)
        //            {
        //                return Problem("Entity set 'MASARDBContext.User'  is null.");
        //            }
        //            _context.User.Add(user);
        //            await _context.SaveChangesAsync();

        //            return CreatedAtAction("GetUser", new { id = user.UserId }, user);
        //        }

        //        DELETE: api/Driver/5
        //        [HttpDelete("{id}")]
        //        public async Task<IActionResult> DeleteUser(int id)
        //        {
        //            if (_context.User == null)
        //            {
        //                return NotFound();
        //            }
        //            var user = await _context.User.FindAsync(id);
        //            if (user == null)
        //            {
        //                return NotFound();
        //            }

        //            _context.User.Remove(user);
        //            await _context.SaveChangesAsync();

        //            return NoContent();
        //        }

        //        private bool UserExists(int id)
        //        {
        //            return (_context.User?.Any(e => e.UserId == id)).GetValueOrDefault();
        //        }
    }
}