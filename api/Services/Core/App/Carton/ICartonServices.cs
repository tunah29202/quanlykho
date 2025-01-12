using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface ICartonServices
    {
        Task<PagedList<CartonResponse>> GetAll(CartonPagedRequest request);
        Task<PagedList<CartonResponse>> GetNotInInvoice(CartonPagedRequest request);
        Task<CartonResponse> GetById(Guid id);
        Task<int> Create(CartonRequest request);
        Task<int> Update(Guid id, CartonRequest request);
        Task<int> Delete(Guid id);
    }
}