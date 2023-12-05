using DevTrack.Infrastructure.Enum;

namespace DevTrack.Web.Models
{
    public class ResponseModel
    {
        public string? Message { get; set; }
        public ResponseTypes Type { get; set; }
    }
}