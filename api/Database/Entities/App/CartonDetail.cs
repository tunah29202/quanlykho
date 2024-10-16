using System;
using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class CartonDetail: BaseEntity
    {
        public Guid carton_id { get; set; }
        [JsonIgnore]
        public Carton carton { get; set; }
        public Guid package_id { get; set; }
        public Package package { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }

        public CartonDetail()
        {
            id = Guid.NewGuid();
        }
    }
}