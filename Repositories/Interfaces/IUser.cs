using MASAR.Model;
using MASAR.Model.DTO;
using System.Security.Claims;

namespace MASAR.Repositories.Interfaces
{
    public interface IUser
    {
        public Task<UserDTO> Login(string username, string password);
        public Task<UserDTO> Logout(string username);
        public Task<UserDTO> GetToken(ClaimsPrincipal claimsPrincipal);
        public Task<ApplicationUser> UpdateUser(string email, ApplicationUser user);
        public Task<List<string>> ViewAnnouncements();
    }
}
