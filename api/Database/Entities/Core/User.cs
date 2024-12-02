using System;
using Database.Common;

namespace Database.Entities
{
    public partial class User : BaseEntity
    {
        public string? code { get; set; }
        public string? user_name { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? hash_password { get; set; }
        public string? salt { get; set; }
        public string? verification_code { get; set; }
        public List<LogAction>? log_actions { get; set; }
        public UserRole user_role { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public virtual ICollection<UserWarehouse>? user_warehouses { get; set; }

        public User()
        {
            id = Guid.NewGuid();
        }
    }
}