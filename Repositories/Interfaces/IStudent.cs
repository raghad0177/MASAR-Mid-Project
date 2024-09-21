using MASAR.Model.DTO;

namespace MASAR.Repositories.Interfaces
{
    public interface IStudent
    {
        public Task<UserDTO> Register(SignUpDTO SignUpDTO);
        public Task<List<string>> ViewAnnouncements();
    }
}
