using Database.Entities;
using FluentValidation;
namespace Services.Core.Contracts
{
    public class PackageRequest
    {
        public string name { get; set; }
        public string origin { get; set; }
        public int quantity { get; set; }
        public double? weight { get; set; }
        public Guid? customer_id { get; set; }
        public Guid warehouse_id { get; set; }
        public string? note { get; set; }
        public List<PackageDetail> package_details { get; set; }
    }
    public class PackageRequestValidator : AbstractValidator<PackageRequest>
    {
        public PackageRequestValidator()
        {
            RuleFor(_ => _.name).MaximumLength(250);
            RuleFor(_ => _.origin).MaximumLength(250);
            RuleFor(_ => _.note).MaximumLength(2048);
            RuleFor(_ => _.warehouse_id);
            RuleFor(_ => _.customer_id);
        }
    }
}
