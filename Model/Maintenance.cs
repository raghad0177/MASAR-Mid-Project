namespace MASAR.Model
{
    public class Maintenance
    {
        public string MaintenanceId { get; set; }
        public string DriverId { get; set; }
        public DriverProfile DriverProfile { get; set; }
        public string BusId { get; set; }
        public Bus Bus {  get; set; }
        public DateTime RequestDate { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
    }
}