namespace MASAR.Model
{
    public class Announcement
    {
        public string AnnouncementId { get; set; }
        public string AdminId { get; set; }
        public ApplicationUser User { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedTime { get; set; }
        public string Audience { get; set; }
    }
}
