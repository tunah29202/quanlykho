    using Database.Common;
    using System.Text.Json.Serialization;

    namespace Database.Entities
    {
        public partial class Customer : BaseEntity
        {
            public string name { get; set; }
            public string company_name { get; set; }
            public string? address { get; set; }
            public string? tax { get; set; }
            public string? tel { get; set; }
            public string? email { get; set; }
            public Guid? user_id { get; set; }
            public User? user { get; set; }
            
            [JsonIgnore]
            public virtual ICollection<Carton>? cartons { get; set; }
            [JsonIgnore]            
            public virtual ICollection<Order>? Orders { get; set; }
            
            public Customer()
            {
                id = Guid.NewGuid();
            }
        }
    }