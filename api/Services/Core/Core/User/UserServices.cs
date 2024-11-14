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
        public UserServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper)
        {
            userRepository = _unitOfWork.GetRepository<User>();
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
                                .FirstOrDefaultAsync();
            var data = _mapper.Map<UserResponse>(user);
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
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
