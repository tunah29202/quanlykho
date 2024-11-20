using Database.Entities;
using FluentValidation;
namespace Services.Core.Contracts
{
    public class OrderDetailRequest
    {
        public Guid order_id { get; set; }
        public Guid product_id { get; set; }
        public int quantity { get; set; }
        public string unit { get; set; }
    }
    public class OrderDetailRequestValidator : AbstractValidator<OrderDetailRequest>
    {
        public OrderDetailRequestValidator()
        {
            RuleFor(_ => _.order_id).NotNull();
            RuleFor(_ => _.product_id).NotNull();
            RuleFor(_ => _.quantity).NotNull();
            RuleFor(_ => _.unit).NotNull();
        }
    }
}
