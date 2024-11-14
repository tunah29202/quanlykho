using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Customer : BaseEntity
    {
        public string code { get; set; }

        public string name { get; set; }

        public string? address { get; set; }

        public string? tel { get; set; }
        [JsonIgnore]
        public virtual ICollection<Carton>? cartons { get; set; }
        public Customer()
        {
            id = Guid.NewGuid();
        }
    }
}