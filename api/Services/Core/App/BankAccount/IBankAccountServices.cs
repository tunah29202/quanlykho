using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IBankAccountServices
    {
        Task<PagedList<BankAccountResponse>> GetAll(PagedRequest request);
        Task<BankAccountResponse> GetById(Guid id);
        Task<int> Create(BankAccountRequest request);
        Task<int> Update(Guid id, BankAccountRequest request);
        Task<int> Delete(Guid id);
    }
}