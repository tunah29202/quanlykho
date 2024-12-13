using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/shipper")]
    [ApiAuthorize]
    [ApiController]
    public class ShipperController : ControllerBase
    {
        private IShipperServices shipperServices { get; set; }
        private readonly ILocalizeServices ls;
        public ShipperController(IShipperServices _shipperServices, ILocalizeServices _ls) : base()
        {
            shipperServices = _shipperServices;
            ls = _ls;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await shipperServices.GetAll(request);
            return Ok(data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await shipperServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] ShipperRequest request)
        {
            int count = await shipperServices.Create(request);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_CREATE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_CREATE) });
            }
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ShipperRequest request)
        {
            int count = await shipperServices.Update(id, request);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_UPDATE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            int count = await shipperServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Shipper, MessageKey.W_DELETE) });
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
    }
}