using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IPaymentMethodServices
    {
        Task<PagedList<PaymentMethodResponse>> GetAll(PagedRequest request);
        Task<PaymentMethodResponse> GetById(Guid id);
        Task<int> Create(PaymentMethodRequest request);
        Task<int> Update(Guid id, PaymentMethodRequest request);
        Task<int> Delete(Guid id);
    }
}