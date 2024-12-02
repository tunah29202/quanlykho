using Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthServices authServices { get; set; }
        private IUserServices userServices { get; set; }
        private ICurrentUserService servicesContext { get; set; }
        private ILocalizeServices ls { get; set; }
        public AuthController(IAuthServices _authServices, IUserServices _userServices, ICurrentUserService _servicesContext, ILocalizeServices _ls) : base()
        {
            servicesContext = _servicesContext;
            authServices = _authServices;
            userServices = _userServices;
            ls = _ls;
        }
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            var res = await authServices.Login(request);
            if (res != null)
                return Ok(res);
            else
                return Unauthorized(res);
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] CustomerRegisterRequest request)
        {
            int count = await userServices.Register(request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.Message, MessageKey.S_CREATE) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Message, MessageKey.E_CREATE) });
        }
        [HttpGet]
        [Route("info")]
        public async Task<IActionResult> GetInfoLogin()
        {
            var data = await userServices.GetInfoLoginById((Guid)servicesContext.user_id);
            if (data != null)
                return Ok(data);
            else
                return BadRequest();
        }

        [HttpPost]
        [Route("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] string refresh_token)
        {
            var res = await authServices.Refresh(refresh_token);
            if (res != null)
                return Ok(res);
            else
                return Unauthorized(res);
        }

        [HttpPut]
        [Route("chang-password")]
        public async Task<IActionResult> ChangePassword([FromBody] AuthChangePassRequest request, Guid id)
        {
            var count = await authServices.ChangePassword(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.ChangePassword, MessageKey.S_CHANGE) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.ChangePassword, MessageKey.E_CHANGE) });
        }
    }
}