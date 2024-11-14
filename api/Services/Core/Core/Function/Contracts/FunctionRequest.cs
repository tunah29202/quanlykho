using FluentValidation;
namespace Services.Core.Contracts
{
    public class FunctionRequest
    {
        public string? code { get; set; }
        public string name { get; set; }
        public string? url { get; set; }
        public string? method { get; set; }
        public string? parent_cd { get; set; }
    }
    public class FunctionRequestValidator : AbstractValidator<FunctionRequest>
    {
        public FunctionRequestValidator()
        {
            RuleFor(x => x.code).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(x => x.name).NotNull().NotEmpty().MaximumLength(250);
        }
    }
}
