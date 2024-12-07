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
    public class CategoryServices : BaseServices, ICategoryServices
    {
        private readonly IRepository<Category> categoryRepository;
        private readonly IRepository<Ingredient> ingredientRepository;
        public CategoryServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            categoryRepository = _unitOfWork.GetRepository<Category>();
            ingredientRepository = _unitOfWork.GetRepository<Ingredient>();
        }

        public async Task<PagedList<CategoryResponse>> GetAll(PagedRequest request)
        {
            PagedList<Category> Categorys;
            if(request.get_all)
            {
                Categorys = categoryRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Categorys = await categoryRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .Include(i=> i.ingredients.Where(i=>i.del_flg==false))
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<CategoryResponse>>(Categorys);
            return dataMapping;
        }

        public async Task<CategoryResponse> GetById(Guid id)
        {
            var Category = await categoryRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(i=> i.ingredients.Where(i=>i.del_flg==false))
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<CategoryResponse>(Category);
            return data;
        }

        public async Task<int> Create(CategoryRequest request)
        {
            var category = _mapper.Map<Category>(request);
            await categoryRepository.AddAsync(category);

            if(request.ingredient_names.Count > 0)
            {
                await _unitOfWork.SaveChangeAsync();
                var category_id = category.id;
                foreach(var ingredient in request.ingredient_names)
                {
                    if(ingredient == null || string.IsNullOrEmpty(ingredient.name))
                    {
                        continue;
                    }
                    await ingredientRepository.AddAsync(new Ingredient()
                    {
                        category_id = category_id,
                        name = ingredient.name
                    });
                }
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, CategoryRequest request)
        {
            var Category = await _unitOfWork
                        .GetRepository<Category>()
                        .GetQuery()
                        .FindActiveById(id)
                        .FirstOrDefaultAsync();
            if(Category == null)
            {
                return -1;
            }
            _mapper.Map(request, Category);
            await categoryRepository.UpdateAsync(Category);
            var oldIngredients = ingredientRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.category_id == id)
                        .ToList();
            if (oldIngredients.Count > 0)
            {
                //remove old ingredient
                foreach (var oldIngredient in oldIngredients)
                {
                    await ingredientRepository.DeleteAsync(oldIngredient);
                }
            }
            if (request.ingredient_names?.Count > 0)
            {
                // update new ingredient
                foreach (var ingredient in request.ingredient_names)
                {
                    if (ingredient == null || string.IsNullOrEmpty(ingredient.name))
                    {
                        continue;
                    }
                    await ingredientRepository.AddAsync(new Ingredient()
                    {
                        category_id = id,
                        name = ingredient.name
                    });
                }
            }

            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Category = await categoryRepository
                                    .GetQuery()
                                    .FindActiveById(id)
                                    .Include(p => p.products.Where(pr => !pr.del_flg))
                                    .FirstOrDefaultAsync();
            if (Category == null)
                return -1;
            if (Category.products != null && Category.products.Count > 0)
                return -2;

            await categoryRepository.DeleteAsync(Category);

            var ingredients = await ingredientRepository
                                        .GetQuery()
                                        .Where(uw => uw.category_id == id)
                                        .ToListAsync();
            foreach (var ingredient in ingredients)
            {
                await ingredientRepository.DeleteAsync(ingredient);
            }

            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
