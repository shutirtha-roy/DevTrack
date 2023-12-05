namespace DevTrack.API.Models
{
    public class LoginResponseModel
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public UserInfo? Data { get; set; }
        public string[]? Errors { get; set; }
    }
}