using FluentValidation;
namespace Services.Core.Contracts
{
    public class InvoiceRequest
    {
        public string invoice_no { get; set; }

        public string? invoice_date { get; set; }
        public string? shipped_per { get; set; }

        public string? shipped_date { get; set; }

        public string? total_weight { get; set; }
        public double? total_volumn { get; set; }

        public Guid shipper_id { get; set; }

        public int status { get; set; }

        public string? notes { get; set; }

        public Guid carton_id { get; set; }

        public string? payment_date { get; set; }

        public Guid? payment_method_id { get; set; }
        public Guid? bank_account_id { get; set; }

        public Guid warehouse_id { get; set; }
        public Guid order_id { get; set; }
    }
    public class InvoiceRequestValidator : AbstractValidator<InvoiceRequest>
    {
        public InvoiceRequestValidator()
        {
            RuleFor(_ => _.invoice_no).NotEmpty().NotNull().MaximumLength(20);
            RuleFor(_ => _.invoice_date).NotEmpty().NotNull();
            RuleFor(_ => _.shipped_per).NotEmpty().NotNull().MaximumLength(250);
            RuleFor(_ => _.shipped_date).NotEmpty().NotNull().MaximumLength(250);
            RuleFor(_ => _.shipped_per).MaximumLength(250);
            RuleFor(_ => _.shipped_date).NotEmpty().NotNull();
            RuleFor(_ => _.total_weight).MaximumLength(20);
            RuleFor(_ => _.shipper_id).NotEmpty();
            RuleFor(_ => _.status).NotEmpty();
            RuleFor(_ => _.carton_id).NotEmpty().NotNull();
            RuleFor(_ => _.order_id).NotEmpty().NotNull();
            RuleFor(_ => _.warehouse_id).NotEmpty().NotNull();
        }
    }
}
