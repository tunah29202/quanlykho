using AutoMapper;
using Common;
using Database.Entities;
using DocumentFormat.OpenXml.Office2010.Excel;
using Helpers.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Services.Core.Services
{
    public class AuthServices : BaseServices, IAuthServices
    {
        private readonly AuthSetting settings;
        private ICurrentUserService currentUserService;
        private readonly Random random = new Random();
        private readonly IRepository<User> userRepository;
        private readonly IRepository<Role> roleRepository;
        private readonly IRepository<UserRole> userRoleRepository;
        public AuthServices(IOptions<AuthSetting> _settings, IUnitOfWork _unitOfWork, IMapper _mapper, ICurrentUserService _currentUserService) : base(_unitOfWork, _mapper)
        {
            settings = _settings.Value;
            userRepository = _unitOfWork.GetRepository<User>();
            roleRepository = _unitOfWork.GetRepository<Role>();
            userRoleRepository = _unitOfWork.GetRepository<UserRole>();
            currentUserService = _currentUserService;
        }
        public async Task<AuthLoginResponse> Login(AuthLoginRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.user_name == request.user_name)
                        .FirstOrDefault();
            if (user == null && !CryptographyProcessor.AreEqual(request.password, user.hash_password, user.salt))
                return null;
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret),
            };
            return data;
        }
        public async Task<AuthLoginResponse> Refresh(string refresh_token)
        {
            var id = JwtHelpers.ValidateToken(refresh_token, settings.JWTSecret);
            if (id == null)
                return null;
            var user = userRepository
                        .GetQuery()
                        .ExcludeSoftDeleted()
                        .FilterById((Guid)id)
                        .FirstOrDefault();
            if (user == null)
                return null;
            var data = new AuthLoginResponse()
            {
                access_token = JwtHelpers.GenerateToken(user, settings.JWTSecret),
                refresh_token = JwtHelpers.GenerateRefreshToken(user, settings.JWTSecret),
            };
            return data;
        }
        public async Task<int> ChangePassword(Guid id, AuthChangePassRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .FindActiveById(id)
                        .FirstOrDefault();
            if (user == null && !CryptographyProcessor.AreEqual(request.current_password, user.hash_password, user.salt))
                return -1;
            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hash_password = CryptographyProcessor.GenerateHash(request.new_password, user.salt);
            await userRepository.UpdateAsync(user);
            return await _unitOfWork.SaveChangeAsync();
        }
        public string GenerateToken()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, 6)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public async Task<int> ForgotPassword(ForgotPasswordRequest request)
        {
            var user = userRepository
                        .GetQuery()
                        .FindActiveById(request.id)
                        .FirstOrDefault();
            if(user == null){
                return -1;
            }
            user.salt = CryptographyProcessor.CreateSalt(20);
            user.hash_password = CryptographyProcessor.GenerateHash(request.new_password, user.salt);
            user.verification_code = null;
            await userRepository.UpdateAsync(user);
            return await _unitOfWork.SaveChangeAsync();
        }
        public bool CheckUserAuthorized(Guid user_id, string path, string action)
        {
            var userRole = userRoleRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .Include(x => x.user)
                            .Where(x => x.user.id == user_id)
                            .Select(x => x.role_cd)
                            .Distinct()
                            .ToList();
            var permissions = roleRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => userRole.Contains(x.code))
                                .Include(x => x.permissions)
                                .SelectMany(x => x.permissions)
                                .ExcludeSoftDeleted()
                                .Include(x => x.function)
                                .Select(x => x.function)
                                .Where(x => !string.IsNullOrEmpty(x.method))
                                .Distinct()
                                .OrderBy(x => x.code)
                                .ToArray();
            var check = permissions.Where(x => x.url.ToLower() == path.ToLower() && x.method.ToUpper() == action)
                                    .FirstOrDefault();
            return check != null ? true : false;
        }

    }
}
