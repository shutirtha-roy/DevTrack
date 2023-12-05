using DevTrack.Infrastructure.Enum;

namespace DevTrack.Infrastructure.BusinessObjects
{
    public class EmailQueue
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public string Template { get; set; }
        public string Subject { get; set; }
        public string? Email { get; set; }
        public EmailSendStatus SendStatus { get; set; }
        public DateTime Date { get; set; }
    }
}