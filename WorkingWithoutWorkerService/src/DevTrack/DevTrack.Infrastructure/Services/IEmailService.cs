namespace DevTrack.Infrastructure.Services
{
    public interface IEmailService
    {
        public Task<bool> SendSingleEmail(string receiverName, string receiverEmail,
            string subject, string body);
    }
}