using Database.Entities;
using FluentValidation;
namespace Services.Core.Contracts
{
    public class CartonDetailRequest
    {
        public Guid carton_id { get; set; }
        public Guid product_id { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
    }
    public class CartonDetailRequestValidator : AbstractValidator<CartonDetailRequest>
    {
        public CartonDetailRequestValidator()
        {
            RuleFor(_ => _.carton_id).NotNull();
            RuleFor(_ => _.product_id).NotNull();
            RuleFor(_ => _.quantity).NotNull();
            RuleFor(_ => _.unit).NotNull();
        }
    }
}
