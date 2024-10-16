using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IPackageServices
    {
        Task<PagedList<PackageResponse>> GetAll(PagedRequest request);
        Task<PackageResponse> GetById(Guid id);
        Task<int> Create(PackageRequest request);
        Task<int> Update(Guid id, PackageRequest request);
        Task<int> Delete(Guid id);
    }
}