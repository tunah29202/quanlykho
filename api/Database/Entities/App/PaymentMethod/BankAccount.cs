using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class BankAccount : BaseEntity
    {
        public Guid payment_method_id { get; set; }
        [JsonIgnore]
        public PaymentMethod? payment_method { get; set; }
        public Guid bank_branch_id { get; set; }
        [JsonIgnore]
        public BankBranch? bank_branch { get; set; }
        public Guid bank_name_id { get; set; }
        [JsonIgnore]
        public BankName? bank_name { get; set; }
        public Guid card_type_id { get; set; }
        [JsonIgnore]
        public CardType? card_type { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice>? invoices { get; set; }
        public string card_number { get; set; }
        public string card_name { get; set; }

        public BankAccount()
        {
            id = Guid.NewGuid();
        }
    }
}