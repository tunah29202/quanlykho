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
    public class ShipperServices : BaseServices, IShipperServices
    {
        private readonly IRepository<Shipper> shipperRepository;
        public ShipperServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            shipperRepository = _unitOfWork.GetRepository<Shipper>();
        }

        public async Task<PagedList<ShipperResponse>> GetAll(PagedRequest request)
        {
            PagedList<Shipper> shippers;
            if(request.get_all)
            {
                shippers = shipperRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                shippers = await shipperRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<ShipperResponse>>(shippers);
            return dataMapping;
        }

        public async Task<ShipperResponse> GetById(Guid id)
        {
            var Shipper = await shipperRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<ShipperResponse>(Shipper);
            return data;
        }
        public async Task<ShipperResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(ShipperRequest request)
        {
            var Shipper = _mapper.Map<Shipper>(request);
            await shipperRepository.AddAsync(Shipper);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, ShipperRequest request)
        {
            var Shipper = await _unitOfWork
                        .GetRepository<Shipper>()
                        .GetByIdAsync(id);
            if(Shipper == null)
            {
                return -1;
            }
            _mapper.Map(request, Shipper);
            await shipperRepository.UpdateAsync(Shipper);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Shipper = await shipperRepository.GetByIdAsync(id);
            if(Shipper == null)
            {
                return -1;
            }
            await shipperRepository.DeleteAsync(Shipper);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
