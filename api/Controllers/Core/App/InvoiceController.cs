using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/Invoice")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private IInvoiceServices invoiceServices { get; set; }
        private readonly ILocalizeServices ls;
        public InvoiceController(IInvoiceServices _invoiceServices, ILocalizeServices _ls) : base()
        {
            invoiceServices = _invoiceServices;
            ls = _ls;
        }
        [ApiAuthorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] InvoicePagedRequest request)
        {
            var data = await invoiceServices.GetAll(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await invoiceServices.GetById(id);
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
        [HttpGet]
        [Route("export-invoice/{id}")]
        public async Task<IActionResult> ExportInvoiceById(Guid id)
        {
            var fileName = $"Invoice_{DateTimeExtention.ToDateTimeStampString(DateTime.Now)}.xlsx";
            var data = await invoiceServices.ExportInvoiceById(id);
            if (data != null)
                return File(data.ToArray(), "application/octetstream", fileName);
            else
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Invoice, MessageKey.TEMPLATE_NOT_FOUND) });
        }
        [ApiAuthorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromBody] InvoiceRequest request)
        {
            int count = await invoiceServices.Create(request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_CREATE) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_CREATE) });
        }
        [ApiAuthorize]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] InvoiceRequest request)
        {
            int count = await invoiceServices.Update(id, request);
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
            int count = await invoiceServices.Delete(id);
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
        [Route("get-invoice-no")]
        public async Task<IActionResult> GetInvoiceNo(string code)
        {
            var data = await invoiceServices.GetInvoiceNo(code);
            if(data != null)
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
        public async Task<IActionResult> GetStatistical(DateTime startDate, DateTime endDate, Guid warehouse_id)
        {
            var data = await invoiceServices.GetStatistical(startDate, endDate, warehouse_id);
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