namespace WebApplication3.Models
{
    public class RequestModel : BaseRequestModel
    {
        public string Token { get; set; }
    }

    public class BaseRequestModel
    {
        public string DeviceId { get; set; }
    }
}