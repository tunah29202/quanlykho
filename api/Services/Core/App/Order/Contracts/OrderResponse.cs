using Database.Entities;
namespace Services.Core.Contracts
{
    public class OrderResponse
    {
        public Guid id { get; set; }
        public string order_no { get; set; }public 
        DateTime order_date { get; set; }
        public string status { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Guid? invoice_id { get; set; }
        public List<OrderDetail> order_details { get; set; }
    }
}