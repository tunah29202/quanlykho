using Database.Entities;
namespace Services.Core.Contracts
{
    public class ProductResponse
    {
        public Guid id { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }

        public string? image { get; set; }

        public string? origin { get; set; }

        public decimal price_unit { get; set; }

        public string unit { get; set; }

        public string? ingredient { get; set; }

        public Guid warehouse_id { get; set; }
        public Warehouse warehouse { get; set; }

        public Guid? category_id { get; set; }

        public Category category { get; set; }
        public string? status { get; set; }
        public int quantity { get; set; }
    }
}