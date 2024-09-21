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
using Microsoft.EntityFrameworkCore.Metadata.Internal;
namespace MASAR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudent _context;
        public StudentController(IStudent context)
        {
            _context = context;
        }
        //GET : api/Student
        [HttpGet("ViewAllAnnouncement")]
        [Authorize(Policy = "RequireStudentRole" )]
        public async Task<List<string>> ViewAllAnnouncement()
        {
            return await _context.ViewAnnouncements();
        }
        // GET: api/Student/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<User>> GetUser(int id)
        //{
        //  if (_context.User == null)
        //  {
        //      return NotFound();
        //  }
        //    var user = await _context.User.FindAsync(id);
        //    if (user == null)
        //    {
        //        return NotFound();
        //    }
        //    return user;
        //}
        //// GET: api/Users/StudentRigester
        [Route("StudentRigester")]
        [HttpPost]
        // for creating Student
        public async Task<ActionResult<UserDTO>> StudentRigester(SignUpDTO user)
        {
            return await _context.Register(user);
        }
    }
}
