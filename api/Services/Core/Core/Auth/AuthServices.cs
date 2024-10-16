using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
using Microsoft.Extensions.Options;
namespace Services.Core.Services
{
    public class AuthServices : BaseServices, IAuthServices
    {
        private readonly AuthSetting settings;
        private readonly IRepository<User> userRepository;
        public AuthServices(IOptions<AuthSetting> _settings, IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            settings = _settings.Value;
            userRepository = _unitOfWork.GetRepository<User>();
        }
        public async Task<AuthLoginResponse> Login(AuthLoginRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.user_name == request.user_name)
                        .FirstOrDefault();
            if(user == null && !CryptographyProcessor.AreEqual(request.password, user.hash_password, user.salt))
            return null;
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret),
            };
            return data;
        }
        public async Task<AuthLoginResponse> Refresh(string refresh_token){
            var id = JwtHelpers.ValidateToken(refresh_token, settings.JWTSecret);
            if (id == null)
            return null;
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .FilterById((Guid)id)
                        .FirstOrDefault();
            if(user==null)
            return null;
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret),
            };
            return data;
        }
        public Task<int> ChangePassword(Guid id, AuthChangePassRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .FindActiveById(id)
                        .FirstOrDefault();
            if(user == null && !CryptographyProcessor.AreEqual(request.current_password, user.hash_password, user.salt))
            return Task.FromResult(0);
            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hash_password = CryptographyProcessor.GenerateHash(request.new_password, user.salt);
            userRepository.UpdateAsync(user);
            return _unitOfWork.SaveChangeAsync();
        }
    }
}
