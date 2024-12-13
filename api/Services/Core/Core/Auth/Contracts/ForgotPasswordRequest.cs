namespace Services.Core.Contracts
{
    public class ForgotPasswordRequest
    {
        public Guid id { get; set; }
        public string new_password { get; set; }
    }
}
