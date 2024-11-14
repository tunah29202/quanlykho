using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IFunctionServices
    {
        Task<PagedList<FunctionResponse>> GetAll(PagedRequest request);
        Task<FunctionResponse> GetById(Guid id);
        Task<int> Create(FunctionRequest request);
        Task<int> Update(Guid id, FunctionRequest request);
        Task<int> Delete(Guid id);
    }
}