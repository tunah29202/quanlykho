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
    public class WarehouseServices : BaseServices, IWarehouseServices
    {
        private readonly IRepository<Warehouse> warehouseRepository;
        public WarehouseServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            warehouseRepository = _unitOfWork.GetRepository<Warehouse>();
        }

        public async Task<PagedList<WarehouseResponse>> GetAll(PagedRequest request)
        {
            PagedList<Warehouse> Warehouses;
            if(request.get_all)
            {
                Warehouses = warehouseRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Warehouses = await warehouseRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<WarehouseResponse>>(Warehouses);
            return dataMapping;
        }

        public async Task<WarehouseResponse> GetById(Guid id)
        {
            var Warehouse = await warehouseRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<WarehouseResponse>(Warehouse);
            return data;
        }

        public async Task<int> Create(WarehouseRequest request)
        {
            var Warehouse = _mapper.Map<Warehouse>(request);
            await warehouseRepository.AddAsync(Warehouse);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, WarehouseRequest request)
        {
            var Warehouse = await _unitOfWork
                                    .GetRepository<Warehouse>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Warehouse == null)
            {
                return -1;
            }
            _mapper.Map(request, Warehouse);
            await warehouseRepository.UpdateAsync(Warehouse);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Warehouse = await warehouseRepository           
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Warehouse == null)
            {
                return -1;
            }
            await warehouseRepository.DeleteAsync(Warehouse);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
