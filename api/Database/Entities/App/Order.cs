using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Order : BaseEntity
    {
        public string? order_no { get; set; }
        public DateTime order_date { get; set; }
        public string status { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public Guid? invoice_id { get; set; }
        [JsonIgnore]
        public Invoice? invoice { get; set; }
        [JsonIgnore]
        public virtual ICollection<OrderDetail>? order_details { get; set; }

        public Order()
        {
            id = Guid.NewGuid();
        }
    }
}
