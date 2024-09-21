namespace MASAR.Model.DTO
{
    public class MaintenancaDTO
    {
        public string MaintenanceId { get; set; }
        public string DriverId { get; set; }
        public string BusId { get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}