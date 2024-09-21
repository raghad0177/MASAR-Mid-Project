namespace MASAR.Model
{
    public class Routing
    {
        public string RoutingId { get; set; }
        public string RouteName { get; set; }
        public string StartPoint { get; set; }
        public string EndPoint { get; set; }
        public int TotalDistance { get; set; }
        public TimeSpan EstimatedTime { get; set; }

        // Relationships
        public ICollection<Schedule> Schedules { get; set; }
    }
}
