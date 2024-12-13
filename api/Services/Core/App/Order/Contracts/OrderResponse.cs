using Database.Entities;
namespace Services.Core.Contracts
{
    public class OrderResponse
    {
        public Guid id { get; set; }
        public string? order_no { get; set; }
        public DateTime order_date { get; set; }
        public int status { get; set; }
        public double total_amount { get; set; }
        public Guid? payment_method_id { get; set; }
        public PaymentMethod payment_method { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public List<OrderDetail>? order_details { get; set; }
        public List<Guid>? warehouse_ids { get; set; }
        public List<string> warehouse_names { get; set; }
    }
}