using System;
using Database.Common;

namespace Database.Entities
{
    public partial class UserWarehouse : BaseEntity
    {
        public Guid user_id { get; set; }

        public User user { get; set; }

        public Guid warehouse_id { get; set; }

        public Warehouse warehouse { get; set; }

        public UserWarehouse()
        {
            id = Guid.NewGuid();
        }
    }
}