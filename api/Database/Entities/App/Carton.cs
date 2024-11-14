using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Carton : BaseEntity
    {
        public string carton_no { get; set; }
        public double net_weight { get; set; }
        public double gross_weight { get; set; }
        public double length { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double volume { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public Guid warehouse_id { get; set; }
        public Warehouse warehouse { get; set; }
        public virtual ICollection<CartonDetail> carton_details { get; set; }
        [JsonIgnore]
        public virtual ICollection<Invoice> invoices { get; set; }

        public Carton()
        {
            id = Guid.NewGuid();
        }
    }
}