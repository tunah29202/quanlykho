using Common;
using Controllers.Common;
using Microsoft.AspNetCore.Mvc;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Controllers.Core
{
    [Route("api/resource")]
    [ApiController]
    public class ResourceController : ControllerBase
    {
        private IResourceServices resourceServices { get; set; }
        private ILocalizeServices ls { get; set; }
        public ResourceController(IResourceServices _resourceServices, ILocalizeServices _ls) : base()
        {
            resourceServices = _resourceServices;
            ls = _ls;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PagedRequest request)
        {
            var data = await resourceServices.GetAll(request);
            return Ok(data);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await resourceServices.GetById(id);
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
        public async Task<IActionResult> Create([FromBody] ResourceRequest request)
        {
            int count = await resourceServices.Create(request);
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
        public async Task<IActionResult> Update(Guid id, [FromBody] ResourceRequest request)
        {
            int count = await resourceServices.Update(id, request);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_UPDATE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_UPDATE) });
            }
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            int count = await resourceServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_DELETE) });
            }
        }

        [HttpPost]
        [Route("import-resource")]
        public async Task<IActionResult> ImportExcel([FromForm] ResourceImportRequest request)
        {
            if (request.file == null || request.file.Length == 0)
            {
                return BadRequest(new { code = ResponseCode.Invalid, message = "File Not Null!" });
            }
            var result = await resourceServices.ImportExcel(request);
            if (result.Item2 != null)
            {
                var fileError = $"insert_products_error_{DateTime.UtcNow.ToString()}.xlsx";
                return File(result.Item2.ToArray(), "application/octetstream", fileError);
            }
            return Ok(result.Item1);
        }
    }
}