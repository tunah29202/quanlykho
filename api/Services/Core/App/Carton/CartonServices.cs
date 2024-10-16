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
    public class CartonServices : BaseServices, ICartonServices
    {
        private readonly IRepository<Carton> cartonRepository;
        public CartonServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            cartonRepository = _unitOfWork.GetRepository<Carton>();
        }

        public async Task<PagedList<CartonResponse>> GetAll(PagedRequest request)
        {
            PagedList<Carton> Cartons;
            if(request.get_all)
            {
                Cartons = cartonRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Cartons = await cartonRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.carton_no.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<CartonResponse>>(Cartons);
            return dataMapping;
        }

        public async Task<CartonResponse> GetById(Guid id)
        {
            var Carton = await cartonRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<CartonResponse>(Carton);
            return data;
        }
        public async Task<CartonResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(CartonRequest request)
        {
            var Carton = _mapper.Map<Carton>(request);
            await cartonRepository.AddAsync(Carton);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, CartonRequest request)
        {
            var Carton = await _unitOfWork
                        .GetRepository<Carton>()
                        .GetByIdAsync(id);
            if(Carton == null)
            {
                return -1;
            }
            _mapper.Map(request, Carton);
            await cartonRepository.UpdateAsync(Carton);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Carton = await cartonRepository.GetByIdAsync(id);
            if(Carton == null)
            {
                return -1;
            }
            await cartonRepository.DeleteAsync(Carton);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
