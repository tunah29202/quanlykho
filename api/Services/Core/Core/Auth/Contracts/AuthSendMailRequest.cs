namespace Services.Core.Contracts
{
    public class AuthSendMailRequest
    {
        public Guid id { get; set; }
        public string email { get; set; }
    }
}
