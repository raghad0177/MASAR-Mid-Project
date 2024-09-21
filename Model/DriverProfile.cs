namespace MASAR.Model
{
    public class DriverProfile
    {
        public string DriverProfileId { get; set; }
        public string DriverId { get; set; }
        public ApplicationUser User { get; set; }
        public int LicenseNumber { get; set; }
        public string BusId { get; set; }
        public Bus Bus { get; set; }
        public string Status { get; set; }
        public ICollection<Maintenance> Maintenances { get; set; }
    }
}