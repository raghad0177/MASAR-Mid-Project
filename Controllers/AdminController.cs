using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MASAR.Data;
using MASAR.Model;
using MASAR.Repositories.Interfaces;
using MASAR.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _context;

        public AdminController(IAdmin context)
        {
            _context = context;
        }
        // GET: api/Users
        [HttpGet("getAllUsers/TEST")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetAllUsers()
        {
            return await _context.GetAllUsers();
        }
        // GET: api/Users
        [Authorize(Policy = "RequireAdminRole")]
        [HttpGet("getAllDriversByAdmin")]
        public async Task<ActionResult<IEnumerable<ApplicationUser>>> GetUser()
        {
            return await _context.GetAllDrivers();
        }
        //// GET: api/Users/5
        [HttpGet("getDriverByEmail/{email}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<ActionResult<ApplicationUser>> GetUser(string email)
        {
            return await _context.GetUserByEmail(email);
        }
        //// GET: api/Users
        [HttpGet("GetAllMaintenanceRequestsByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<List<Maintenance>> GetAllMaintenanceRequests()
        {
            return await _context.GetAllMaintenanceRequests();
        }
        // PUT: api/Users
        [HttpPut("ApprovedMaintenanceRequestByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO)
        {
            return await _context.ApprovedMaintenanceRequest(ifApprove, maintenanceDTO);
        }
        //POST: api/Users
        //To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("CreateDriverByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        // for adding driver
        public async Task<ActionResult<ApplicationUser>> PostUser(SignUpDTO user)
        {
            return await _context.CreateUser(user);
        }
        [HttpPost("CreateAnnouncementByAdmin")]
        [Authorize(Policy = "RequireAdminRole")]
        // for adding driver
        public async Task<ActionResult<Announcement>> CreatAnnouncement(string email,AnnouncementDTO announcement)
        {
            return await _context.CreateAnnouncement(email, announcement);
        }
        //DELETE: api/Users/5
        [HttpDelete("DeleteDriverByAdmin/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        //for deleting driver
        public async Task<IActionResult> DeleteDriver(string email)
        {
            try
            {
                await _context.DeleteUser(email); // Await the async operation
                return Ok(); // Return a 200 OK response if successful
            }
            catch (Exception e)
            {
                // Log and return a 500 Internal Server Error response
                Console.WriteLine($"An error occurred: {e.Message}");
                return StatusCode(500, $"Internal server error: {e.Message}");
            }
        }    
    }
}