using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class OrderDetail : BaseEntity
    {
        public Order order { get; set; }
        public Guid order_id { get; set; }
        public Product product { get; set; }
        public Guid product_id { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }

        public OrderDetail()
        {
            id = Guid.NewGuid();
        }
    }
}
