using Database.Common;
using System.Text.Json.Serialization;

namespace Database.Entities
{
    public partial class Ingredient : BaseEntity
    {
        public string name { get; set; }
        public Guid category_id { get; set; }
        [JsonIgnore]
        public Category? category { get; set; }

        public Ingredient()
        {
            id = Guid.NewGuid();
        }
    }
}