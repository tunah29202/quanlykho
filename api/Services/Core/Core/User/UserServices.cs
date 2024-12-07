using AutoMapper;
using Common;
using Database.Entities;
using Helpers.Auth;
using Microsoft.EntityFrameworkCore;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Services.Core.Services
{
    public class UserServices : BaseServices, IUserServices
    {
        private readonly IRepository<User> userRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        private readonly IRepository<UserWarehouse> userWarehouseRepository;
        private readonly IRepository<Customer> customerRepository;
        public UserServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            userRepository = _unitOfWork.GetRepository<User>();
            userRoleRepository = _unitOfWork.GetRepository<UserRole>();
            userWarehouseRepository = _unitOfWork.GetRepository<UserWarehouse>();
            customerRepository = _unitOfWork.GetRepository<Customer>();
        }

        public async Task<PagedList<UserResponse>> GetAll(PagedRequest request)
        {
            PagedList<User> data;
            var users = await userRepository.GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => !string.IsNullOrEmpty(request.search) ? x.full_name.ToLower().Contains(request.search.ToLower()) : true)
                                .SortBy(request.sort ?? "updated_at.desc")
                                .ToPagedListAsync(request.page, request.size);
            var dataMapping = _mapper.Map<PagedList<UserResponse>>(users);
            return dataMapping;
        }

        public async Task<UserResponse> GetById(Guid id)
        {
            var user = await userRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .Include(x => x.user_role)
                                    .ThenInclude(y=> y.role)
                                .Include(u => u.user_warehouses!.Where(uw => uw.warehouse != null))
                                    .ThenInclude(uw => uw.warehouse)
                                .FirstOrDefaultAsync();
            var data = _mapper.Map<UserResponse>(user);
            data.role_cd = user.user_role.role_cd;
            data.role_name = user.user_role.role.name;
            if(user.user_warehouses != null)
            {
                data.warehouse_ids = user.user_warehouses?.Select(w => w.warehouse_id).ToList();
                data.warehouse_names = user.user_warehouses?.Select(w => w.warehouse.name).ToList();
            }
            return data;
        }
        public async Task<UserResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(UserRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hash_password = CryptographyProcessor.GenerateHash(request.password, user.salt);
            await userRepository.AddAsync(user);
            if(request.role_cd != null)
            {
                await userRoleRepository.AddAsync(new UserRole()
                {
                    user_id = user.id,
                    role_cd = request.role_cd
                });
            }
            if(request.warehouse_ids != null)
            {
                foreach(var warehouse_id in request.warehouse_ids)
                {
                    await userWarehouseRepository.AddAsync(new UserWarehouse()
                    {
                        user_id = user.id,
                        warehouse_id = warehouse_id,
                    });
                }
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Register(CustomerRegisterRequest request)
        {
            var user = _mapper.Map<User>(request);
            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hash_password = CryptographyProcessor.GenerateHash(request.password, user.salt);
            var customer = new Customer()
            {
                name = request.full_name,
                company_name = request.company_name,
                address = request.address,
                tax = request.tax,
                tel = request.phone,
                email = request.email,
            };
            await customerRepository.AddAsync(customer);
            user.customer_id = customer.id;
            await userRepository.AddAsync(user);
            await userRoleRepository.AddAsync(new UserRole()
                {
                    user_id = user.id,
                    role_cd = "CUSTOMER"
                });
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, UserRequest request)
        {
            var user = await _unitOfWork
                                .GetRepository<User>()
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .FirstOrDefaultAsync();
            if (user == null)
            {
                return -1;
            }
            _mapper.Map(request, user);
            await userRepository.UpdateAsync(user);

            var oldRole = await userRoleRepository.GetQuery().FirstOrDefaultAsync(ur => ur.user_id == id);
            if(oldRole != null)
            {
                oldRole.role_cd = request.role_cd;
            }
            else
            {
                await userRoleRepository.AddAsync(new UserRole()
                {
                    user_id = id,
                    role_cd = request.role_cd
                });
            }
            var oldWarehouses = userWarehouseRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => x.user_id == id)
                                .ToList();
            //delete old warehouse
            foreach(var oldWarehouse in oldWarehouses)
            {
                await userWarehouseRepository.DeleteAsync(oldWarehouse);
            }
            if(request.warehouse_ids!=null && request.warehouse_ids.Count > 0)
            {
                foreach(var userWarehouse in request.warehouse_ids)
                {
                    var oldWarehouse = await userWarehouseRepository
                                                .GetQuery()
                                                .FirstOrDefaultAsync(uw => uw.warehouse_id == userWarehouse && uw.del_flg);
                    if(oldWarehouse != null )
                    {
                        oldWarehouse.user_id = id;
                        oldWarehouse.del_flg = false;
                    }
                    else
                    {
                        await userWarehouseRepository.AddAsync(new UserWarehouse()
                        {
                            user_id = id,
                            warehouse_id = userWarehouse
                        });
                    }
                }
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var user = await userRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .FilterById(id)
                                .FirstOrDefaultAsync();
            if (user == null)
            {
                return -1;
            }
            await userRepository.DeleteAsync(user);
            var userWarehouses = await userWarehouseRepository.GetQuery().Where(uw => uw.user_id == id).ToListAsync();
            foreach(var userWarehouse in userWarehouses)
            {
                await userWarehouseRepository.DeleteAsync(userWarehouse);
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
