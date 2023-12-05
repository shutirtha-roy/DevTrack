namespace DevTrack.Infrastructure.BusinessObjects
{
    public class UpdatePassword
    {
        public Guid Id { get; set; }
        public string? OldPassword { get; set; }
        public string? NewPassword { get; set; }
    }
}