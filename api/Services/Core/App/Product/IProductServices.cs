using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IProductServices
    {
        Task<PagedList<ProductResponse>> GetAll(PagedRequest request);
        Task<ProductResponse> GetById(Guid id);
        Task<int> Create(ProductRequest request);
        Task<int> Update(Guid id, ProductRequest request);
        Task<int> Delete(Guid id);
    }
}