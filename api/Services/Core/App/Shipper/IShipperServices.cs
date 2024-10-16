using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IShipperServices
    {
        Task<PagedList<ShipperResponse>> GetAll(PagedRequest request);
        Task<ShipperResponse> GetById(Guid id);
        Task<int> Create(ShipperRequest request);
        Task<int> Update(Guid id, ShipperRequest request);
        Task<int> Delete(Guid id);
    }
}