using Database.Common;
using Database.Entities;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class PaymentMethod : BaseEntity
    {
        public string? name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice>? invoices { get; set; }
        [JsonIgnore]
        public virtual ICollection<Order>? orders { get; set; }

        public PaymentMethod()
        {
            id = Guid.NewGuid();
        }
    }
}