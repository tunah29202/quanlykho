using FluentValidation;
namespace Services.Core.Contracts
{
    public class BankAccountRequest
    {
        public string? code { get; set; }
        public string? BankAccount_name { get; set; }
        public string? full_name { get; set; }
        public string? gender { get; set; }
        public string? email { get; set; }
        public string? phone { get; set; }
        public string? password { get; set; }
    }
    public class BankAccountRequestValidator : AbstractValidator<BankAccountRequest>
    {
        public BankAccountRequestValidator()
        {
            RuleFor(x=>x.code).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x=>x.BankAccount_name).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(x=>x.full_name).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(x=>x.gender).NotNull().NotEmpty();
            RuleFor(x=>x.email).NotNull().NotEmpty().MaximumLength(200);
            RuleFor(x=>x.email).NotNull().NotEmpty().MaximumLength(20);
        }
    }
}
