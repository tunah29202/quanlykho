using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Package : BaseEntity
    {
        public string name { get; set; }
        public string origin { get; set; }
        public int quantity { get; set; }
        public double? weight { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public Guid warehouse_id { get; set; }
        [JsonIgnore]
        public Warehouse warehouse { get; set; }
        public string? note { get; set; }
        public virtual ICollection<PackageDetail> package_details { get; set; }
        public virtual ICollection<CartonDetail> carton_details { get; set; }

        public Package()
        {
            id = Guid.NewGuid();
            name = "";
            origin = "";
            note = "";
            quantity = 0;
            warehouse_id= Guid.NewGuid();
        }
    }   
}