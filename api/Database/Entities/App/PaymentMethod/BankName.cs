using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class BankName : BaseEntity
    {
        public string? name { get; set; }
        [JsonIgnore]
        public virtual ICollection<BankAccount>? bank_accounts { get; set; }
        public BankName()
        {
            id = Guid.NewGuid();
        }
    }
}