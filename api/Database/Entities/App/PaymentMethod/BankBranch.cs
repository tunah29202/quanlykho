using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class BankBranch : BaseEntity
    {
        public string? name { get; set; }
        [JsonIgnore]
        public virtual ICollection<BankAccount>? bank_accounts { get; set; }
        public BankBranch()
        {
            id = Guid.NewGuid();
        }
    }
}