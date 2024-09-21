using MASAR.Data;
using MASAR.Model;
using MASAR.Model.DTO;
using MASAR.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;
namespace MASAR.Repositories.Services
{
    public class Admin : IAdmin
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MASARDBContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        // inject jwt service
        private JwtTokenService _jwtTokenService;
        public Admin(UserManager<ApplicationUser> Manager,
            SignInManager<ApplicationUser> signInManager, JwtTokenService jwtTokenService, MASARDBContext context)
        {
            _userManager = Manager;
            _signInManager = signInManager;
            _jwtTokenService = jwtTokenService;
            _context = context;
        }
        public async Task<List<ApplicationUser>> GetAllUsers()
        {
            try
            {
                var allUSers = await _context.Users.ToListAsync();
                return allUSers;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<ApplicationUser>> GetAllDrivers()
        {
            try
            {
                var allUSers = await _context.Users.Where(a => a.UserType == "Driver").ToListAsync();
                return allUSers;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<List<Maintenance>> GetAllMaintenanceRequests()
        {
            try
            {
                var allRequests = await _context.Maintenance.ToListAsync();
                return allRequests;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<Maintenance> ApprovedMaintenanceRequest(string ifApprove, MaintenancaDTO maintenanceDTO)
        {
            var maintenance = await _context.Maintenance.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var bus = await _context.DriverProfile.Where(e => e.BusId == maintenanceDTO.BusId).FirstOrDefaultAsync();
            var driver = await _userManager.FindByIdAsync( bus.DriverId);
            if (ifApprove == "Yes")
            {
                maintenance.Status = "Approved";
                bus.Status = "Under Maintenance";
                _context.Entry(maintenance).State = EntityState.Modified;
                _context.Entry(bus).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                SendOtpViaEmail("Your Request For The Maintenance Has Been Approved", driver.Email, "Maintenance Approval");
            }
            return maintenance;
        }
        public async Task<ApplicationUser> GetUserByEmail(string email)
        {
            try
            {
                var user = await _context.ApplicationUser.Where(a => a.Email == email).FirstOrDefaultAsync();
                return user;
            }
            catch (NotImplementedException e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
        public async Task<ApplicationUser> CreateUser(SignUpDTO signUpDto)
        {
            var phone = int.Parse(signUpDto.Phone);
            var addedUser = new ApplicationUser
            {
                UserName = signUpDto.UserName,
                Email = signUpDto.UserEmail,
                UserType = "Driver",
                PhoneNumber = phone.ToString(),
                Roles = "Driver"
            };
            var resultWithPass = await _userManager.CreateAsync(addedUser, signUpDto.Password);
            await _userManager.AddToRolesAsync(addedUser, signUpDto.Roles);
            await _context.SaveChangesAsync();
            return addedUser;
        }
        public async Task<Announcement> CreateAnnouncement(string email ,AnnouncementDTO AnnouncementDto)
        {
                var addedAnnouncement = new Announcement
                {
                    AnnouncementId = AnnouncementDto.AnnouncementId,
                    AdminId = AnnouncementDto.AdminId,
                    Audience = AnnouncementDto.Audience,
                    Title = AnnouncementDto.Title,
                    Content = AnnouncementDto.Content,
                    CreatedTime = DateTime.Now
                };
                _context.Announcement.Add(addedAnnouncement);
                await _context.SaveChangesAsync();
                return addedAnnouncement;            
        }
        public async Task DeleteUser(string email)
        {
            try
            {
                var user = await _context.ApplicationUser.FindAsync(email);
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                _context.ApplicationUser.Remove(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                // Log the exception
                Console.WriteLine($"An error occurred while deleting the user: {e.Message}");
                throw;
            }
        }
        void SendOtpViaEmail(string mess, string email, string subject)
        {
            // Create a new instance of MailMessage class
            MailMessage message = new MailMessage();
            // Set subject of the message, body and sender information
            message.Subject = subject;
            message.Body = mess;
            message.From = new MailAddress("ayawahidi@outlook.com", "Admin");
            // Add To recipients and CC recipients
            message.To.Add(new MailAddress(email, "Recipient 1"));
            // Create an instance of SmtpClient class
            SmtpClient client = new SmtpClient();
            // Specify your mailing Host, Username, Password, Port # and Security option
            client.Host = "smtp.office365.com";
            client.Credentials = new NetworkCredential("ayawahidi@outlook.com", "Roro2001**");
            client.Port = 587;
            client.EnableSsl = true;
            try
            {
                // Send this email
                client.Send(message);
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.ToString());
            }
        }
    }
}
