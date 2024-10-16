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
    public class ProductServices : BaseServices, IProductServices
    {
        private readonly IRepository<Product> productRepository;
        public ProductServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            productRepository = _unitOfWork.GetRepository<Product>();
        }

        public async Task<PagedList<ProductResponse>> GetAll(PagedRequest request)
        {
            PagedList<Product> Products;
            if(request.get_all)
            {
                Products = productRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Products = await productRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<ProductResponse>>(Products);
            return dataMapping;
        }

        public async Task<ProductResponse> GetById(Guid id)
        {
            var Product = await productRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<ProductResponse>(Product);
            return data;
        }
        public async Task<ProductResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(ProductRequest request)
        {
            var Product = _mapper.Map<Product>(request);
            await productRepository.AddAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, ProductRequest request)
        {
            var Product = await _unitOfWork
                        .GetRepository<Product>()
                        .GetByIdAsync(id);
            if(Product == null)
            {
                return -1;
            }
            _mapper.Map(request, Product);
            await productRepository.UpdateAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Product = await productRepository.GetByIdAsync(id);
            if(Product == null)
            {
                return -1;
            }
            await productRepository.DeleteAsync(Product);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
