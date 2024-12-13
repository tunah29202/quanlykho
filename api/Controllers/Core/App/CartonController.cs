using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
using Services.Core.Services;
namespace Controllers.Core
{
    [Route("api/Carton")]
    [ApiController]
    public class CartonController : ControllerBase
    {
        private ICartonServices cartonServices { get; set; }
        private readonly ILocalizeServices ls;
        public CartonController(ICartonServices _cartonServices, ILocalizeServices _ls) : base()
        {
            cartonServices = _cartonServices;
            ls = _ls;
        }
        [ApiAuthorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CartonPagedRequest request)
        {
            var data = await cartonServices.GetAll(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("get-not-in-invoice")]
        public async Task<IActionResult> GetNotInInvoice([FromQuery] CartonPagedRequest request)
        {
            var data = await cartonServices.GetNotInInvoice(request);
            return Ok(data);
        }

        [ApiAuthorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await cartonServices.GetById(id);
            if (data != null)
            {
                return Ok(data.ToResponse());
            }
            else
            {
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [ApiAuthorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] CartonRequest request)
        {
            int count = await cartonServices.Create(request);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] CartonRequest request)
        {
            int count = await cartonServices.Update(id, request);
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
            int count = await cartonServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Carton, MessageKey.W_DELETE) });
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
    }
}