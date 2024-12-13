using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserServices userServices { get; set; }
        private readonly ILocalizeServices ls;
        public UserController(IUserServices _userServices, ILocalizeServices _ls) : base()
        {
            userServices = _userServices;
            ls = _ls;
        }
        [ApiAuthorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await userServices.GetAll(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await userServices.GetById(id);
            if (data != null)
            {
                return Ok(data.ToResponse());
            }
            else
            {
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] UserRequest request)
        {
            int count = await userServices.Create(request);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_CREATE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_CREATE) });
            }
        }
        [ApiAuthorize]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserRequest request)
        {
            int count = await userServices.Update(id, request);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_UPDATE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [ApiAuthorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            int count = await userServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [HttpGet]
        [Route("get-user")]
        public async Task<IActionResult> GetUserByNameEmail(string user_name, string email)
        {
            var data = await userServices.GetByUserNameEmail(user_name, email);
            if (data != null)
                return Ok(data.ToResponse());
            else
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.User, MessageKey.U_NOT_FOUND) });
        }
    }
}