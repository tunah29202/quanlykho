using Common;
using Helpers.Email;
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
        private IEmailHelpers emailHelpers { get; set; }
        private IUserServices userServices { get; set; }
        private ICurrentUserService servicesContext { get; set; }
        private ILocalizeServices ls { get; set; }
        public AuthController(IAuthServices _authServices, IUserServices _userServices, 
            ICurrentUserService _servicesContext, ILocalizeServices _ls, IEmailHelpers _emailHelpers) : base()
        {
            servicesContext = _servicesContext;
            authServices = _authServices;
            userServices = _userServices;
            emailHelpers = _emailHelpers;
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

        [HttpPost]
        [Route("send-mail")]
        public async Task <IActionResult> SendMail([FromBody] AuthSendMailRequest request)
        {
            var pass_code = authServices.GenerateToken();
            var body_email = "This is to regenerate your password: " + pass_code;
            var subject = "Forgot password";
            bool send_mail = emailHelpers.SendEmailWithBody(body_email, request.email, subject);
            if (send_mail)
            {
                var save_code = await userServices.SaveCode(request.id, pass_code);
                if(save_code < 0)
                {
                    return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.E_GET_CODE) });
                }
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.S_SEND_MAIl) });
            }
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.E_SEND_MAIL) });
        }

        [HttpPost]
        [Route("check-code")]
        public async Task<IActionResult> CheckCode([FromBody] AuthCheckCodeRequest request)
        {
            var check_code = await userServices.CheckCode(request);
            if (check_code)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.S_CHECK_CODE) });
            }
            return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.E_CHECK_CODE) });
        }
        [HttpPut]
        [Route("forgot_password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var count = await authServices.ForgotPassword(request);
            if (count>=1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.S_FORGOT) });
            }
            return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.ForgotPassword, MessageKey.E_FORGOT) });
        }
    }
}