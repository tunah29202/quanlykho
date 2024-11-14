using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IRoleServices
    {
        Task<PagedList<RoleResponse>> GetAll(PagedRequest request);
        Task<RoleResponse> GetById(Guid id);
        Task<int> Create(RoleRequest request);
        Task<int> Update(Guid id, RoleRequest request);
        Task<int> Delete(Guid id);
    }
}