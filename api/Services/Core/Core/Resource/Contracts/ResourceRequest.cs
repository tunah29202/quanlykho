using FluentValidation;
namespace Services.Core.Contracts
{
    public class ResourceRequest
    {
        public string lang { get; set; }
        public string module { get; set; }
        public string screen { get; set; }
        public string key { get; set; }
        public string text { get; set; }
    }
    public class ResourceRequestValidator : AbstractValidator<ResourceRequest>
    {
        public ResourceRequestValidator()
        {
            RuleFor(x=>x.lang).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(x=>x.module).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(x=>x.screen).NotNull().NotEmpty().MaximumLength(250);
            RuleFor(x=>x.key).NotNull().NotEmpty();
            RuleFor(x=>x.text).NotNull().NotEmpty().MaximumLength(200);
        }
    }
}
