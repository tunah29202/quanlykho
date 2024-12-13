using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/warehouse")]
    [ApiAuthorize]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private IWarehouseServices warehouseServices { get; set; }
        private readonly ILocalizeServices ls;
        public WarehouseController(IWarehouseServices _warehouseServices, ILocalizeServices _ls) : base()
        {
            warehouseServices = _warehouseServices;
            ls = _ls;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await warehouseServices.GetAll(request);
            return Ok(data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await warehouseServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] WarehouseRequest request)
        {
            int count = await warehouseServices.Create(request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_CREATE) });
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Warehouse, MessageKey.I_DUPLICATE_004) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_CREATE) });
        }

        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] WarehouseRequest request)
        {
            int count = await warehouseServices.Update(id, request);
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
            int count = await warehouseServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Warehouse, MessageKey.W_DELETE) });
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
    }
}