using Database.Entities;
namespace Services.Core.Contracts
{
    public class PackageResponse
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string origin { get; set; }
        public int quantity { get; set; }
        public double? weight { get; set; }
        public Guid? customer_id { get; set; }
        public Customer? customer { get; set; }
        public Guid warehouse_id { get; set; }
        public Warehouse warehouse { get; set; }
        public string? note { get; set; }
        public List<PackageDetail> package_details { get; set; }
    }
}