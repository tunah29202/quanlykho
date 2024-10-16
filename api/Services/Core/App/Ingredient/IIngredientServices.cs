using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IIngredientServices
    {
        Task<PagedList<IngredientResponse>> GetAll(PagedRequest request);
        Task<IngredientResponse> GetById(Guid id);
        Task<int> Create(IngredientRequest request);
        Task<int> Update(Guid id, IngredientRequest request);
        Task<int> Delete(Guid id);
    }
}