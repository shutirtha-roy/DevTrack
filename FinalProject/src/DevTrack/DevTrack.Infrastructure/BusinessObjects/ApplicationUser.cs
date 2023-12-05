namespace DevTrack.Infrastructure.BusinessObjects
{
    public class ApplicationUser
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string? Image { get; set; }
        public string? TimeZone { get; set; }
    }
}