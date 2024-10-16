using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IConsigneeServices
    {
        Task<PagedList<ConsigneeResponse>> GetAll(PagedRequest request);
        Task<ConsigneeResponse> GetById(Guid id);
        Task<int> Create(ConsigneeRequest request);
        Task<int> Update(Guid id, ConsigneeRequest request);
        Task<int> Delete(Guid id);
    }
}