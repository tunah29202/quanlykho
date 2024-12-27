using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
using Services.Core.Services;
namespace Controllers.Core
{
    [Route("api/Order")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IOrderServices orderServices { get; set; }
        private readonly ILocalizeServices ls;
        public OrderController(IOrderServices _orderServices, ILocalizeServices _ls) : base()
        {
            orderServices = _orderServices;
            ls = _ls;
        }
        [ApiAuthorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] OrderPagedRequest request)
        {
            var data = await orderServices.GetAll(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("get-not-in-invoice")]
        public async Task<IActionResult> GetNotInInvoice([FromQuery] OrderPagedRequest request)
        {
            var data = await orderServices.GetNotInInvoice(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await orderServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            int count = await orderServices.Create(request);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] OrderRequest request)
        {
            int count = await orderServices.Update(id, request);
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
            int count = await orderServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Order, MessageKey.W_DELETE) });
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [HttpGet]
        [Route("get-order-no")]
        public async Task<IActionResult> GetOrderNo(string code)
        {
            var data = await orderServices.GetOrderNo(code);
            if (data != null)
            {
                return Ok(data.ToResponse());
            }
            else
            {
                return BadRequest(data?.ToResponse());
            }
        }
        [HttpGet]
        [Route("statistical")]
        public async Task<IActionResult> GetStatistical(DateTime startDate, DateTime endDate, Guid? warehouse_id)
        {
            var data = await orderServices.GetStatistical(startDate, endDate, warehouse_id);
            if (data != null)
            {
                return Ok(data.ToResponse());
            }
            else
            {
                return BadRequest(data?.ToResponse());
            }
        }
    }
}