using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
using DocumentFormat.OpenXml.Wordprocessing;
namespace Services.Core.Services
{
    public class WarehouseServices : BaseServices, IWarehouseServices
    {
        private readonly IRepository<Warehouse> warehouseRepository;
        private readonly IRepository<UserWarehouse> userWarehouseRepository;
        private readonly IRepository<OrderWarehouse> orderWarehouseRepository;
        private ICurrentUserService serviceContext { get; set; }
        public WarehouseServices(IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserService _serviceContext) : base(_unitOfWork, _mapper) 
        { 
            warehouseRepository = _unitOfWork.GetRepository<Warehouse>();
            userWarehouseRepository = _unitOfWork.GetRepository<UserWarehouse>();
            orderWarehouseRepository = _unitOfWork.GetRepository<OrderWarehouse>();
            serviceContext = _serviceContext;
        }

        public async Task<PagedList<WarehouseResponse>> GetAll(PagedRequest request)
        {
            PagedList<Warehouse> Warehouses;
            if(request.get_all)
            {
                var query = warehouseRepository
                            .GetQuery()
                            .ExcludeSoftDeleted();
                List<Guid> userWarehouseIds = new List<Guid>();
                if(serviceContext.user_id != null)
                {
                    userWarehouseIds = await userWarehouseRepository
                                                .GetQuery()
                                                .ExcludeSoftDeleted()
                                                .Where(uw=> uw.user_id == (Guid)serviceContext.user_id)
                                                .Select(uw=>uw.warehouse_id)
                                                .ToListAsync();
                    query = query.Where(w => userWarehouseIds.Contains(w.id));
                }
                Warehouses = query 
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
            var warehouse = warehouseRepository
                .GetQuery()
                .FindActiveById(id)
                .FirstOrDefault();

            if (warehouse == null)
                return -1;

            var wh_product = await _unitOfWork.GetRepository<Product>()
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(p => p.warehouse_id == id)
                .FirstOrDefaultAsync();

            if (wh_product != null)
                return -2;

            var wh_carton = await _unitOfWork.GetRepository<Carton>()
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(p => p.warehouse_id == id)
                .FirstOrDefaultAsync();

            if (wh_carton != null)
                return -2;


            var wh_invoice = await _unitOfWork.GetRepository<Invoice>()
                .GetQuery()
                .ExcludeSoftDeleted()
                .Where(p => p.warehouse_id == id)
                .FirstOrDefaultAsync();

            if (wh_invoice != null)
                return -2;

            await warehouseRepository.DeleteAsync(warehouse);

            var oldUserWarehouse = await userWarehouseRepository
                                        .GetQuery()
                                        .ExcludeSoftDeleted()
                                        .Where(x => x.warehouse_id == id)
                                        .ToListAsync();

            foreach (var item in oldUserWarehouse)
            {
                await userWarehouseRepository.DeleteAsync(item);
            }
            var oldOrderWarehouse = await orderWarehouseRepository
                                        .GetQuery()
                                        .ExcludeSoftDeleted()
                                        .Where(x => x.warehouse_id == id)
                                        .ToListAsync();

            foreach (var item in oldOrderWarehouse)
            {
                await orderWarehouseRepository.DeleteAsync(item);
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
