using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace MASAR.Repositories.Services
{
    public class IdentityAccountService : IUser
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public IdentityAccountService(UserManager<ApplicationUser> Manager, 
            SignInManager<ApplicationUser> signInManager ,JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<UserDTO> Login(string name, string password)
        {
            var user = await _userManager.FindByNameAsync(name);
            bool passvalidation = await _userManager.CheckPasswordAsync(user, password);
            var result = new UserDTO()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Token = await _jwtTokenService.GenerateToken(user, System.TimeSpan.FromMinutes(60))
            };
            return result;
        }    
        public async Task<UserDTO> Logout(string email)
        {
            var logoutAccount = await _userManager.FindByEmailAsync(email);
            if (logoutAccount == null)
            {
                // Handle user not found case
                return null;
            }
            // Clear the authentication cookie or token here
            await _signInManager.SignOutAsync();
            var result = new UserDTO()
            {
                UserId = logoutAccount.Id,
                UserName = logoutAccount.UserName
            };
            return result;
        }
      
        public async Task<UserDTO> GetToken(ClaimsPrincipal claimsPrincipal)
        {
            var user = await _userManager.GetUserAsync(claimsPrincipal);
            if (user == null)
            {
                throw new InvalidOperationException("Token Is Not Exist!");
            }
            return new UserDTO()
            {
                UserName = user.UserName,
                Token = await _jwtTokenService.GenerateToken(user, System.TimeSpan.FromMinutes(60))//just for development purposes
            };
        }      
        public async Task<List<string>> ViewAnnouncements()
        {
          var allAnnoun = await _context.Announcement.Where(a => a.Audience == "All")
                .Select(a => "Title: " + a.Title + "   Content: " + a.Content + "    Created Time: " + a.CreatedTime).ToListAsync();

          return allAnnoun;
        }
        public async Task<ApplicationUser> UpdateUser(string email, ApplicationUser user)
        {
            try
            {
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return user;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
      
    }
}