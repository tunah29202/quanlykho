using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Shipper : BaseEntity
    {
        public string name { get; set; }

        public string address { get; set; }

        public string tel { get; set; }

        public string? fax { get; set; }

        public string? email { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice> invoices { get; set; }

        public Shipper()
        {
            id = Guid.NewGuid();
        }
    }
}