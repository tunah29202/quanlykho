using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
namespace Services.Core.Services
{
    public class IngredientServices : BaseServices, IIngredientServices
    {
        private readonly IRepository<Ingredient> ingredientRepository;
        public IngredientServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            ingredientRepository = _unitOfWork.GetRepository<Ingredient>();
        }

        public async Task<PagedList<IngredientResponse>> GetAll(PagedRequest request)
        {
            PagedList<Ingredient> Ingredients;
            if(request.get_all)
            {
                Ingredients = ingredientRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Ingredients = await ingredientRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<IngredientResponse>>(Ingredients);
            return dataMapping;
        }

        public async Task<IngredientResponse> GetById(Guid id)
        {
            var Ingredient = await ingredientRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<IngredientResponse>(Ingredient);
            return data;
        }
        public async Task<IngredientResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(IngredientRequest request)
        {
            var Ingredient = _mapper.Map<Ingredient>(request);
            await ingredientRepository.AddAsync(Ingredient);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, IngredientRequest request)
        {
            var Ingredient = await _unitOfWork
                        .GetRepository<Ingredient>()
                        .GetByIdAsync(id);
            if(Ingredient == null)
            {
                return -1;
            }
            _mapper.Map(request, Ingredient);
            await ingredientRepository.UpdateAsync(Ingredient);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Ingredient = await ingredientRepository.GetByIdAsync(id);
            if(Ingredient == null)
            {
                return -1;
            }
            await ingredientRepository.DeleteAsync(Ingredient);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
