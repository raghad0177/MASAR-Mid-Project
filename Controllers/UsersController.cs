using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUser _context;

        public UsersController(IUser context)
        {
            _context = context;
        }
        [HttpPost("Login")]
        //[Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<UserDTO>> Login(string username, string password)
        {
            var newLogin = await _context.Login(username, password);
            return newLogin;
        }
        [HttpPost("Logout")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<ActionResult<UserDTO>> Logout(string username)
        {
            var newLogout = await _context.Logout(username);
            return newLogout;
        }
        //GET : api/Users
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireUserRole")]
        public async Task<List<string>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncements();
        }
        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("updateUserByEmail/{email}")]
        [Authorize(Policy = "RequireAdminStudentRole")]
        public async Task<ActionResult<ApplicationUser>> PutUser(string email, ApplicationUser user)
        {
          return await _context.UpdateUser(email, user);
        }
    }
}