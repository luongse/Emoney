namespace WebApplication3.Models
{
    public class BaseRequestModel
    {
        public string DeviceId { get; set; }
    }
    public class RequestModel : BaseRequestModel
    {
        public string Token { get; set; }
        public string WalletId { get; set; }
    }

   

    public class ApiSendMoneyInfoRquest : RequestModel
    {
        public string Amount { get; set; }
        public string Content { get; set; }
        public int Currency { get; set; }
        public int Option { get; set; }
        public string Pin { get; set; }
        public string ReceiverMsisdn { get; set; }
    }

    public class ApiConfirmSendMoneyInfoRquest : RequestModel
    {
        public string TransId { get; set; }
      
    }
}