namespace Services.Core.Contracts
{
    public class AuthCheckCodeRequest
    {
        public Guid id { get; set; }
        public string code { get; set; }
    }
}
