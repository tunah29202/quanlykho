using FluentValidation;
namespace Services.Core.Contracts
{
    public class ShipperRequest
    {
        public string name { get; set; }

        public string address { get; set; }

        public string? email { get; set; }

        public string tel { get; set; }

        public string? fax { get; set; }
    }
    public class ShipperRequestValidator : AbstractValidator<ShipperRequest>
    {
        public ShipperRequestValidator()
        {
            RuleFor(_ => _.name).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.address).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.email).MaximumLength(250);
            RuleFor(_ => _.tel).NotNull().NotEmpty().MaximumLength(15);
            RuleFor(_ => _.fax).MaximumLength(15);
        }
    }
}
