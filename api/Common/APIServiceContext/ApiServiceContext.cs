using Database.Entities;
using Helpers.Auth;
using Microsoft.Extensions.Options;
using Services.Core.Contracts;

namespace Common
{
    public class ApiServiceContext : ICurrentUserService
    {
        public Guid? user_id { get; set; }
        public string user_name { get; set; }
        public string full_name { get; set; }
        private IOptions<AuthSetting> _settings {  get; set; }
        private IHttpContextAccessor _contextAccessor { get; set; }
        public ApiServiceContext(IOptions<AuthSetting> settings, IHttpContextAccessor contextAccessor)
        {
            _settings = settings;
            _contextAccessor = contextAccessor;
            try
            {
                var bearerToken = contextAccessor.HttpContext.Request.Headers["Authorization"];
                var token = !string.IsNullOrEmpty(bearerToken) ? bearerToken.ToString().Substring("Bearer ".Length) : null;
                var claims = JwtHelpers.GetClaimsByValidateToken(token, settings.Value.JWTSecret);
                if (claims != default)
                {
                    var _user_id = claims.FirstOrDefault(x => x.Type == nameof(User.id));
                    if(_user_id != default)
                    {
                        user_id = Guid.Parse(_user_id.Value);
                    }

                    var _user_name = claims.FirstOrDefault(x => x.Type == nameof(User.user_name));
                    if (_user_name != default)
                    {
                        user_name = _user_name.Value;
                    }

                    var _full_name = claims.FirstOrDefault(x => x.Type == nameof(User.full_name));
                    if (_full_name != default)
                    {
                        full_name = _full_name.Value;
                    }
                }
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
