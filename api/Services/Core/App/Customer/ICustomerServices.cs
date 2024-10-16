using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface ICustomerServices
    {
        Task<PagedList<CustomerResponse>> GetAll(PagedRequest request);
        Task<CustomerResponse> GetById(Guid id);
        Task<int> Create(CustomerRequest request);
        Task<int> Update(Guid id, CustomerRequest request);
        Task<int> Delete(Guid id);
    }
}