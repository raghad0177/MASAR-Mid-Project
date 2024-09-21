using MASAR.Model.DTO;
using MASAR.Model;
using Microsoft.AspNetCore.Identity;
using MASAR.Data;
using MASAR.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MASAR.Repositories.Services
{
    public class Student : IStudent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Student(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public Task<List<string>> ViewAnnouncements()
        {
            var allAnnoun = _context.Announcement.Where(a => a.Audience == "Student").Select(a => "Title: " + a.Title + "    Content: " + a.Content + "   Created Time: " + a.CreatedTime).ToListAsync();
            return allAnnoun;
        }
        public async Task<UserDTO> Register(SignUpDTO signUpDTO)
        {
            var user = new ApplicationUser()
            {
                UserName = signUpDTO.UserName,
                Email = signUpDTO.UserEmail,
                UserType = "Student",
                PhoneNumber = signUpDTO.Phone,
                Roles = "Student"                
            };
            // Create the user with the provided password
            var resultWithPass = await _userManager.CreateAsync(user, signUpDTO.Password);
            // Check if user creation was successful
            await _userManager.AddToRolesAsync(user, signUpDTO.Roles);
            await _context.SaveChangesAsync();        
            // Create a DTO to return
            var result = new UserDTO()
            {
                UserId = user.Id,
                UserName = user.UserName
            };
            return result;
        }
    }
}
