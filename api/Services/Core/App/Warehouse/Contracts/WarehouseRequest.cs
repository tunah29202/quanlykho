using FluentValidation;
namespace Services.Core.Contracts
{
    public class WarehouseRequest
    {
        public string code { get; set; }
        public string name { get; set; }
        public string address { get; set; }
        public string tel { get; set; }
    }
    public class WarehouseRequestValidator : AbstractValidator<WarehouseRequest>
    {
        public WarehouseRequestValidator()
        {
            RuleFor(_ => _.code).NotNull().NotEmpty().MaximumLength(10);
            RuleFor(_ => _.name).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.address).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.tel).NotNull().NotEmpty().MaximumLength(15);
        }
    }
}
