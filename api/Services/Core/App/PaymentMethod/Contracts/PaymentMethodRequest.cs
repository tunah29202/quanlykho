using FluentValidation;
namespace Services.Core.Contracts
{
    public class PaymentMethodRequest
    {
        public string? name { get; set; }
    }
    public class PaymentMethodRequestValidator : AbstractValidator<PaymentMethodRequest>
    {
        public PaymentMethodRequestValidator()
        {
            RuleFor(x => x.name).NotNull().NotEmpty().MaximumLength(250);
        }
    }
}
