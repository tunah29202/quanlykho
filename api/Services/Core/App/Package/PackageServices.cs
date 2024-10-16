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
    public class PackageServices : BaseServices, IPackageServices
    {
        private readonly IRepository<Package> packageRepository;
        public PackageServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            packageRepository = _unitOfWork.GetRepository<Package>();
        }

        public async Task<PagedList<PackageResponse>> GetAll(PagedRequest request)
        {
            PagedList<Package> Packages;
            if(request.get_all)
            {
                Packages = packageRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Packages = await packageRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<PackageResponse>>(Packages);
            return dataMapping;
        }

        public async Task<PackageResponse> GetById(Guid id)
        {
            var Package = await packageRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<PackageResponse>(Package);
            return data;
        }
        public async Task<PackageResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(PackageRequest request)
        {
            var Package = _mapper.Map<Package>(request);
            await packageRepository.AddAsync(Package);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, PackageRequest request)
        {
            var Package = await _unitOfWork
                        .GetRepository<Package>()
                        .GetByIdAsync(id);
            if(Package == null)
            {
                return -1;
            }
            _mapper.Map(request, Package);
            await packageRepository.UpdateAsync(Package);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Package = await packageRepository.GetByIdAsync(id);
            if(Package == null)
            {
                return -1;
            }
            await packageRepository.DeleteAsync(Package);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
