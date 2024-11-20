using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IOrderServices
    {
        Task<PagedList<OrderResponse>> GetAll(PagedRequest request);
        Task<OrderResponse> GetById(Guid id);
        Task<int> Create(OrderRequest request);
        Task<int> Update(Guid id, OrderRequest request);
        Task<int> Delete(Guid id);
    }
}