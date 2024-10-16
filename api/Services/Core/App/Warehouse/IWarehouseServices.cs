using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IWarehouseServices
    {
        Task<PagedList<WarehouseResponse>> GetAll(PagedRequest request);
        Task<WarehouseResponse> GetById(Guid id);
        Task<int> Create(WarehouseRequest request);
        Task<int> Update(Guid id, WarehouseRequest request);
        Task<int> Delete(Guid id);
    }
}