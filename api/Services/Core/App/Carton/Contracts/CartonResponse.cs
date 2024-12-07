using Database.Entities;
namespace Services.Core.Contracts
{
    public class CartonResponse
    {
        public Guid id { get; set; }
        public string carton_no { get; set; }
        public double net_weight { get; set; }
        public double gross_weight { get; set; }
        public double length { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double volume { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public List<CartonDetail> carton_details { get; set; }
    }
}