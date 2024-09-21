
using MASAR.Model.DTO;
using MASAR.Model;

namespace MASAR.Repositories.Interfaces
{
    public interface IBus
    {
        public Task<Bus> CreateBusInfo(BusDTO busDTO);
    }
}