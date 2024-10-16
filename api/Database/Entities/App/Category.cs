using System;
using Database.Common;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Category : BaseEntity
    {
        public string name { get; set; }
        public virtual ICollection<Ingredient>? ingredients { get; set; }
        [JsonIgnore]
        public virtual ICollection<Product>? products { get; set; }

        public Category()
        {
            id = Guid.NewGuid();
        }
    }
}