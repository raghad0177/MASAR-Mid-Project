namespace MASAR.Model.DTO
{
    public class ScheduleDTO
    {
        public int ScheduleId { get; set; }
        public int DriverId { get; set; }
        public int RoutingId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime EstimatedTime { get; set; }
        public string Status { get; set; }
    }
}
