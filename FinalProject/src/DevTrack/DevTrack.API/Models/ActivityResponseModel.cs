namespace DevTrack.API.Models
{
    public class ActivityResponseModel
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string[]? Data { get; set; }
        public string[]? Errors { get; set; }
    }
}