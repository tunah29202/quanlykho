using System;
using Database.Common;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Product : BaseEntity
    {

        public string? code { get; set; }
        public string? name { get; set; }

        public string? image { get; set; }

        public string? origin { get; set; }

        public decimal price_unit { get; set; }

        [NotMapped]
        public string unit { get; set; }

        public string? ingredient { get; set; }

        public Guid warehouse_id { get; set; }
        [JsonIgnore]
        public Warehouse warehouse { get; set; }

        public Guid? category_id { get; set; }

        public Category category { get; set; }
        public string? status { get; set; }
        public int quantity { get; set; }
        [JsonIgnore]
        public virtual ICollection<CartonDetail>? carton_details { get; set; }
        public virtual ICollection<OrderDetail>? order_details { get; set; }

        public Product()
        {
            id = Guid.NewGuid();
        }
    }
}