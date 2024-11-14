using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class CartonDetail : BaseEntity
    {
        public Guid carton_id { get; set; }
        [JsonIgnore]
        public Carton carton { get; set; }
        public Guid product_id { get; set; }
        public Product product { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }

        public CartonDetail()
        {
            id = Guid.NewGuid();
        }
    }
}