using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthLoginResponse> Login(AuthLoginRequest request);
        Task<AuthLoginResponse> Refresh(string refresh_token);
        Task<int> ChangePassword(Guid id, AuthChangePassRequest request);
    }
}