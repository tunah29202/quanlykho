using System;
using Database.Common;

namespace Database.Entities
{
    public partial class PackageDetail : BaseEntity
    {
        public Guid package_id { get; set; }
        public Package package { get; set; }
        public Guid product_id { get; set; }
        public Product product { get; set; }
        public int quantity { get; set; }
        public string? unit { get; set; }

        public PackageDetail() 
        {
            package_id = Guid.NewGuid();
            product_id = Guid.NewGuid();
        }
    }
}