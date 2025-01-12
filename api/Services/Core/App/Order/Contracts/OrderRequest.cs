using Database.Entities;
using FluentValidation;
namespace Services.Core.Contracts
{
    public class OrderRequest
    {
        public string order_no { get; set; }
        public string? order_date { get; set; }
        public int? status { get; set; }
        public double total_amount { get; set; }
        public Guid? customer_id { get; set; }
        public Guid? invoice_id { get; set; }
        public Guid? payment_method_id { get; set; }
        public List<Guid>? warehouse_ids { get; set; }
        public List<OrderDetailRequest> order_details { get; set; }
    }
    public class OrderRequestValidator : AbstractValidator<OrderRequest>
    {
        public OrderRequestValidator()
        {
            RuleFor(_ => _.order_no).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(_ => _.order_date).NotNull();
            RuleFor(_ => _.status).NotNull();
            RuleFor(_ => _.total_amount).NotNull();
            RuleFor(_ => _.customer_id).NotNull();
        }
    }
}
