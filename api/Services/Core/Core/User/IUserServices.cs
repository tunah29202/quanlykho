using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IUserServices
    {
        Task<PagedList<UserResponse>> GetAll(PagedRequest request);
        Task<UserResponse> GetById(Guid id);
        Task<UserResponse> GetInfoLoginById(Guid id);
        Task<int> Create(UserRequest request);
        Task<int> Update(Guid id, UserRequest request);
        Task<int> Delete(Guid id);
    }
}