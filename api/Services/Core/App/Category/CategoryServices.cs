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
        public CategoryServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            categoryRepository = _unitOfWork.GetRepository<Category>();
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
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<CategoryResponse>>(Categorys);
            return dataMapping;
        }

        public async Task<CategoryResponse> GetById(Guid id)
        {
            var Category = await categoryRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<CategoryResponse>(Category);
            return data;
        }
        public async Task<CategoryResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(CategoryRequest request)
        {
            var Category = _mapper.Map<Category>(request);
            await categoryRepository.AddAsync(Category);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, CategoryRequest request)
        {
            var Category = await _unitOfWork
                        .GetRepository<Category>()
                        .GetByIdAsync(id);
            if(Category == null)
            {
                return -1;
            }
            _mapper.Map(request, Category);
            await categoryRepository.UpdateAsync(Category);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Category = await categoryRepository.GetByIdAsync(id);
            if(Category == null)
            {
                return -1;
            }
            await categoryRepository.DeleteAsync(Category);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
