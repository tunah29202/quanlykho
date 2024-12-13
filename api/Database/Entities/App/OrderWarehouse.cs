using System;
using Database.Common;

namespace Database.Entities
{
    public partial class OrderWarehouse : BaseEntity
    {
        public Guid order_id { get; set; }

        public Order order { get; set; }

        public Guid warehouse_id { get; set; }

        public Warehouse warehouse { get; set; }

        public OrderWarehouse()
        {
            id = Guid.NewGuid();
        }
    }
}