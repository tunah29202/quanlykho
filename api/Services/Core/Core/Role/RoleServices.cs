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
    public class RoleServices : BaseServices, IRoleServices
    {
        private readonly IRepository<Role> roleRepository;
        public RoleServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            roleRepository = _unitOfWork.GetRepository<Role>();
        }

        public async Task<PagedList<RoleResponse>> GetAll(PagedRequest request)
        {
            PagedList<Role> data;
            var Roles = await roleRepository.GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                .SortBy(request.sort ?? "updated_at.desc")
                                .ToPagedListAsync(request.page, request.size);
            var dataMapping = _mapper.Map<PagedList<RoleResponse>>(Roles);
            return dataMapping;
        }

        public async Task<RoleResponse> GetById(Guid id)
        {
            var Role = await roleRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .FirstOrDefaultAsync();
            var data = _mapper.Map<RoleResponse>(Role);
            return data;
        }

        public async Task<int> Create(RoleRequest request)
        {
            var Role = _mapper.Map<Role>(request);
            await roleRepository.AddAsync(Role);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, RoleRequest request)
        {
            var Role = await _unitOfWork
                                .GetRepository<Role>()
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .FirstOrDefaultAsync();
            if(Role == null)
            {
                return -1;
            }
            _mapper.Map(request, Role);
            await roleRepository.UpdateAsync(Role);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Role = await roleRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .FirstOrDefaultAsync();
            if(Role == null)
            {
                return -1;
            }
            await roleRepository.DeleteAsync(Role);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
