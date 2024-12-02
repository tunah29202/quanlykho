using FluentValidation;
namespace Services.Core.Contracts
{
    public class ProductExportRequest
    {
        public string? image { get; set; }
        public string? code { get; set; }
        public string? name { get; set; }
        public string? origin { get; set; }
        public decimal? price_unit { get; set; }
        public int? quantity { get; set; }
        public string? status { get; set; }
        public string? category_name { get; set; }
        public string? ingredient { get; set; }
        public Guid? warehouse_id { get; set; }
        public Guid? category_id { get; set; }
    }
    public class ProductExportRequestValidator : AbstractValidator<ProductExportRequest>
    {
        public ProductExportRequestValidator()
        {
            RuleFor(_ => _.name).MaximumLength(250);
            RuleFor(_ => _.code).MaximumLength(250);
            RuleFor(_ => _.origin).MaximumLength(250);
            RuleFor(_ => _.status);
            RuleFor(_ => _.quantity);
        }
    }
}
