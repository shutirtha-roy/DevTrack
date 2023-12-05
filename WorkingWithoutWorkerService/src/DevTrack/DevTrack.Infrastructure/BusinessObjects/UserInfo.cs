namespace DevTrack.Infrastructure.BusinessObjects
{
    public class UserInfo
    {
        public Guid UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}