using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface ICategoryServices
    {
        Task<PagedList<CategoryResponse>> GetAll(PagedRequest request);
        Task<CategoryResponse> GetById(Guid id);
        Task<int> Create(CategoryRequest request);
        Task<int> Update(Guid id, CategoryRequest request);
        Task<int> Delete(Guid id);
    }
}