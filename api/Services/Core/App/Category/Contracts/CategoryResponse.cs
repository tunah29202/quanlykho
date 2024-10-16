using Database.Entities;
namespace Services.Core.Contracts
{
    public class CategoryResponse
    {
        public Guid id { get; set; }
        public string? name { get; set; }
        public List<Ingredient>? ingredients{ get; set; }
    }
}