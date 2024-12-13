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
        private readonly IRepository<Permission> permissionRepository;
        public RoleServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            roleRepository = _unitOfWork.GetRepository<Role>();
            permissionRepository = _unitOfWork.GetRepository<Permission>();
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
                                .Include(x => x.permissions.Where(y => y.del_flg == false))
                                .ThenInclude(p => p.function)
                                .ThenInclude(z => z.parent)
                                .ThenInclude(z => z.children)
                                .FirstOrDefaultAsync();
            var data = _mapper.Map<RoleResponse>(Role);
            return data;
        }

        public async Task<int> Create(RoleRequest request)
        {
            var Role = _mapper.Map<Role>(request);
            await roleRepository.AddAsync(Role);
            await ConfigPermissions(Role.code, request.permissions);
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
            await ConfigPermissions(Role.code, request.permissions);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Role = await roleRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .Include(x => x.user_roles.Where(x => x.del_flg == false))
                                .FirstOrDefaultAsync();
            if(Role == null)
            {
                return -1;
            }
            if (Role.user_roles != null&& Role.user_roles.Count()>0)
            {
                return -2;
            }
            await roleRepository.DeleteAsync(Role);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
        private async Task ConfigPermissions(string code, List<string> request)
        {
            var permissions = _unitOfWork.GetRepository<Function>()
                                            .GetQuery()
                                            .ExcludeSoftDeleted()
                                            .Where(x => request.Contains(x.code))
                                            .ToArray();
            var permissions_codes = permissions.Select(y => y.code).ToArray();

            var oldPermissions = permissionRepository.GetQuery().Where(p => p.role_cd == code).ToArray();
            foreach(var item in oldPermissions)
            {
                if(permissions_codes.Contains(item.function_cd))
                {
                    item.del_flg = false;
                    await permissionRepository.UpdateAsync(item);
                }
                else
                {
                    await permissionRepository.DeleteAsync(item);
                }
            } 
            var newPermisssions = permissions.Where(x => !oldPermissions.Select(y => y.function_cd).ToArray().Contains(x.code)).ToArray();
            foreach(var item in newPermisssions)
            {
                var newEntity = new Permission()
                {
                    role_cd = code,
                    function_cd = item.code
                };
                await permissionRepository.AddAsync(newEntity);
            }
        }
    }
}
