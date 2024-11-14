using FluentValidation;

namespace Services.Core.Contracts
{
    public class ResourceImportRequest
    {
        public IFormFile file { get; set; }
        public class ResourceImportRequestValidator : AbstractValidator<ResourceImportRequest>
        {
            public ResourceImportRequestValidator()
            {
                RuleFor(x => x.file).NotNull().NotEmpty();
            }
        }
    }
}