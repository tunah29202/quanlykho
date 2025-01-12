using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IOrderServices
    {
        Task<PagedList<OrderResponse>> GetAll(OrderPagedRequest request);
        Task<PagedList<OrderResponse>> GetNotInInvoice(OrderPagedRequest request);
        Task<OrderResponse> GetById(Guid id);
        Task<int> Create(OrderRequest request);
        Task<int> Update(Guid id, OrderRequest request);
        Task<int> Delete(Guid id);
        Task<string> GetOrderNo(string code);
        Task<StatisticalResponse> GetStatistical(DateTime startDate, DateTime endDate, Guid? warehouse_id);

    }
}