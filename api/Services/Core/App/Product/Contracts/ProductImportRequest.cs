using FluentValidation;
namespace Services.Core.Contracts
{
    public class ProductImportRequest
    {
        public IFormFile? file { get; set; }
        public Guid warehouse_id { get; set; }
    }
    public class ProductImportRequestValidator : AbstractValidator<ProductImportRequest>
    {
        public ProductImportRequestValidator()
        {
            RuleFor(_ => _.file).NotNull().NotEmpty();
            RuleFor(_ => _.warehouse_id).NotNull().NotEmpty();
        }
    }
}
