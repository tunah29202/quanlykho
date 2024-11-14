using Database.Entities;
using FluentValidation;
namespace Services.Core.Contracts
{
    public class CartonRequest
    {
        public string carton_no { get; set; }
        public double net_weight { get; set; }
        public double gross_weight { get; set; }
        public double length { get; set; }
        public double height { get; set; }
        public double width { get; set; }
        public double volume { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Guid warehouse_id { get; set; }
        public List<CartonDetailRequest> carton_details { get; set; }
    }
    public class CartonRequestValidator : AbstractValidator<CartonRequest>
    {
        public CartonRequestValidator()
        {
            RuleFor(_ => _.carton_no).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(_ => _.net_weight).NotNull();
            RuleFor(_ => _.gross_weight).NotNull();
            RuleFor(_ => _.length).NotNull();
            RuleFor(_ => _.height).NotNull();
            RuleFor(_ => _.width).NotNull();
            RuleFor(_ => _.volume).NotNull();
            RuleFor(_ => _.warehouse_id).NotNull().NotEmpty();
        }
    }
}
