using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class PaymentMethod : BaseEntity
    {
        public string? name { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice>? invoices { get; set; }
        [JsonIgnore]
        public virtual ICollection<BankAccount>? bank_accounts { get; set; }

        public PaymentMethod()
        {
            id = Guid.NewGuid();
        }
    }
}