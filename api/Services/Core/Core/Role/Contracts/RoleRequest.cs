using FluentValidation;
namespace Services.Core.Contracts
{
    public class RoleRequest
    {
        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public List<string>? permissions { get; set; }
    }
    public class RoleRequestValidator : AbstractValidator<RoleRequest>
    {
        public RoleRequestValidator()
        {
            RuleFor(_ => _.code).NotEmpty().NotNull().MaximumLength(50);
            RuleFor(_ => _.name).NotEmpty().NotNull().MaximumLength(250);
            RuleFor(_ => _.description).MaximumLength(500);
        }
    }
}
