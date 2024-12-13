namespace Services.Core.Contracts
{
    public class ProductPagedRequest
    {
        public int page { get; set; } = 1;

        public int size { get; set; } = 10;

        public string? sort { get; set; }

        public string? search { get; set; }

        public string? category_name { get; set; }

        public bool get_all { get; set; }

        public string? product_ids_in_carton { get; set; }

        public string? created_at { get; set; }

        public Guid warehouse_id { get; set; }
    }
}