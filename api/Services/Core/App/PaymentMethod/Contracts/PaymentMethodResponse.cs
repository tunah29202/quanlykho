using Database.Entities;
namespace Services.Core.Contracts
{
    public class PaymentMethodResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? user_name { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
    }
}