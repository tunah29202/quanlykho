using FluentValidation;
namespace Services.Core.Contracts
{
    public class CustomerRequest
    {
        public string? code { get; set; }
        public string? name { get; set; }
        public string? company_name { get; set; }
        public string? company_type { get; set; }
        public string? address { get; set; }
        public string? tax { get; set; }
        public string? tel { get; set; }
        public string? email { get; set; }
    }
    public class CustomerRequestValidator : AbstractValidator<CustomerRequest>
    {
        public CustomerRequestValidator()
        {
            RuleFor(_ => _.code).NotNull().NotEmpty().MaximumLength(10);
            RuleFor(_ => _.name).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.address).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(_ => _.tel).MaximumLength(15);
        }
    }
}
