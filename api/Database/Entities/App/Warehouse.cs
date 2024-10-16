using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Warehouse : BaseEntity
    {
        public string code { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public string? tel { get; set; }
        [JsonIgnore]
        public virtual ICollection<UserWarehouse>? user_warehouses { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? products { get; set; }
        [JsonIgnore]
        public virtual ICollection<Package>? packages { get; set; }
        [JsonIgnore]
        public virtual ICollection<Carton>? cartons { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice>? invoices { get; set; }

        public Warehouse()
        {
            id = Guid.NewGuid();
        }
    }
}