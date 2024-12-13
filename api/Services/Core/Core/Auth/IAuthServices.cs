using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IAuthServices
    {
        Task<AuthLoginResponse> Login(AuthLoginRequest request);
        Task<AuthLoginResponse> Refresh(string refresh_token);
        Task<int> ChangePassword(Guid id, AuthChangePassRequest request);
        Task<int> ForgotPassword(ForgotPasswordRequest request);
        string GenerateToken();
        bool CheckUserAuthorized(Guid user_id, string path, string action);
    }
}