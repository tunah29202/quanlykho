using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Services.Core.Contracts;
using Services.Core.Interfaces;
using Helpers;
using Common;
using Controllers.Common;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml;
using Helpers.Excel;
using Database.Entities;
using DocumentFormat.OpenXml.Spreadsheet;
namespace Controllers.Core
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IProductServices productServices { get; set; }
        private readonly ILocalizeServices ls;
        private readonly IWebHostEnvironment env;
        private readonly string _imageStoragePath = "Images";
        private readonly string Template = "Template";

        public ProductController(IProductServices _productServices, ILocalizeServices _ls, IWebHostEnvironment _env) : base()
        {
            productServices = _productServices;
            ls = _ls;
            env = _env;
        }
        [ApiAuthorize]
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ProductPagedRequest request)
        {
            var data = await productServices.GetAll(request);
            return Ok(data);
        }
        [HttpPost]
        [Route("getProductInventory")]
        public async Task<IActionResult> GetProductInventory([FromBody] ProductPagedRequest request)
        {
            var data = await productServices.GetAll(request);
            return Ok(data);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var data = await productServices.GetById(id);
            if (data != null)
            {
                return Ok(data.ToResponse());
            }
            else
            {
                return BadRequest(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND)});
            }
        }
        [ApiAuthorize]
        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> Create([FromForm] ProductRequest request)
        {
            int count = await productServices.Create(request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_CREATE) });
            else if (count == -1)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Product, MessageKey.I_DUPLICATE_004) });
            else if (count == -2)
                return Ok(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Product, MessageKey.FILE_NOT_FOUND) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.E_CREATE) });
        }
        [ApiAuthorize]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> Update(Guid id, [FromForm] ProductRequest request)
        {
            int count = await productServices.Update(id, request);
            if (count >= 1)
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_UPDATE) });
            else if (count == -1)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Product, MessageKey.I_DUPLICATE_004) });
            else if (count == -2)
                return Ok(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, Screen.Product, MessageKey.FILE_NOT_FOUND) });
            else
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
        }
        [ApiAuthorize]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            int count = await productServices.Delete(id);
            if (count >= 1)
            {
                return Ok(new { code = ResponseCode.Success, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.S_DELETE) });
            }
            else if (count == -2)
                return Ok(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Product, MessageKey.W_DELETE) });
            else
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            }
        }
        [HttpGet]
        [Route("download-file-template")]
        public IActionResult DownloadTemplate()
        {
            string template = Path.Combine(env.WebRootPath, Template);
            string templatePath = Path.Combine(template, "IMPORT_PRODUCT_TEMPLATE.xlsx");
            if (!System.IO.File.Exists(templatePath))
            {
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Product, MessageKey.TEMPLATE_NOT_FOUND) });
            }
            using(MemoryStream stream = new MemoryStream())
            {
                using(var file = new FileStream(templatePath, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(stream);
                }
                stream.Position = 0;
                return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "modifiled_template.xlsx");
            }
        }
        [ApiAuthorize]
        [HttpPost]
        [Route("import-product")]
        public async Task<IActionResult> ImportExcel([FromForm] ProductImportRequest request)
        {
            if(request.file == null || request.file.Length == 0)
                return BadRequest(new { code = ResponseCode.Invalid, message = ls.Get(Modules.Core, Screen.Product, MessageKey.FILE_NOT_NULL) });
            var result = await productServices.ImportExcel(request);
            if (result.Item2 != null)
            {
                var fileError = $"insert_product_error_{DateTimeExtention.ToDateTimeStampString(DateTime.Now)}.xlsx";
                return File(result.Item2.ToArray(), "application/octetstream", fileError);
            }
            return Ok(result.Item1);
        }
        [ApiAuthorize]
        [HttpGet]
        [Route("export-product")]
        public async Task<IActionResult> ExportExcel([FromQuery] ProductPagedRequest request)
        {
            var fileName = $"Product_{DateTimeExtention.ToDateTimeStampString(DateTime.Now)}.xlsx";
            var directory = Path.Combine(env.WebRootPath, Template);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            };
            var filePathDownload = Path.Combine(directory, Template);
            var products = await productServices.GetAll(request);
            if (products == null)
                return Ok(new { code = ResponseCode.NotFound, message = ls.Get(Modules.Core, ScreenKey.COMMON, MessageKey.NOT_FOUND) });
            var imageStoragePath = Path.Combine(env.WebRootPath, _imageStoragePath);
            using (var ms = new MemoryStream())
            {
                using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
                {
                    document.CreateWorkbookPart();
                    WorkbookPart? workbookPart = document.WorkbookPart;
                    workbookPart?.CreateSheet("Sheet1");

                    List<ExcelItem> excelItems = new ()
                    {
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "image_text"), key = "image", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 30, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "code_text"), key = "code", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 15, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "name_text"), key = "name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 30, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "origin_text"), key = "origin", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 15, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "status_text"), key = "status", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 10, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "price_unit_text"), key = "price_unit", type = DataType.DECIMAL, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 15, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "quantity_text"), key = "quantity", type = DataType.NUMBER, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 15, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "category_name_text"), key = "category_name", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 15, isKeyIncluded = true},
                        new ExcelItem(){ header = ls.Get(Modules.Core, "Product", "ingredient_text"), key = "ingredient", type = DataType.TEXT, header_align = CellAlign.CENTER, content_align = CellAlign.LEFT, width = 20, isKeyIncluded = true},
                    };

                    List<ProductExportRequest> exportProducts = new List<ProductExportRequest>();
                    foreach (var product in products.data)
                    {
                        var exportProduct = new ProductExportRequest
                        {
                            image = "",
                            code = product.code,
                            name = product.name,
                            origin = product.origin,
                            status = product.status,
                            price_unit = product.price_unit,
                            quantity = product.quantity,
                            ingredient = product.ingredient,
                            category_name = product.category != null? product.category.name: "",
                        };
                        if (!string.IsNullOrEmpty(product.image))
                        {
                            var filePathImage = Path.Combine(imageStoragePath, product.image);
                            if (System.IO.File.Exists(filePathImage))
                            {
                                exportProduct.image = product.image;
                            }
                        }
                        exportProducts.Add(exportProduct);
                    }

                    workbookPart?.FillGridData(exportProducts.ToList(), excelItems);
                }
                System.IO.File.WriteAllBytes(filePathDownload, ms.ToArray());
            }
            if (!System.IO.File.Exists(filePathDownload))
                return BadRequest(new { code = ResponseCode.SystemError, message = ls.Get(Modules.Core, Screen.Product, MessageKey.E_EXPORT) });
            using (SpreadsheetDocument doc = SpreadsheetDocument.Open(filePathDownload, true))
            {
                WorkbookPart? workbookPart = doc.WorkbookPart;

                //Read the first Sheet from Excel file.
                Sheet? sheet = workbookPart?.Workbook?.Sheets?.GetFirstChild<Sheet>();

                WorksheetPart? worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheet?.Id);
                //Get the Worksheet instance.
                Worksheet? worksheet = worksheetPart.Worksheet;

                IEnumerable<Row>? rows = worksheet?.GetFirstChild<SheetData>()?.Descendants<Row>();

                int rowIndex = 0;
                int countRow = rows != null ? rows.Count() : 0;
                foreach (Row row in rows)
                {
                    rowIndex++;
                    if (rowIndex > countRow)
                        break;
                    if (row != null)
                    {
                        if (rowIndex > 2)
                        {
                            var (rowCheck, filePath) = ExcelImport.CheckValueRow(doc, row, 0, rowIndex);
                            string imgDesc = Path.GetFileNameWithoutExtension(filePath);
                            if (rowCheck > 0)
                            {
                                row.CustomHeight = true;
                                row.Height = new DoubleValue((double)130);
                                
                                var filePathImage = Path.Combine(imageStoragePath, filePath);
                                if (System.IO.File.Exists(filePathImage))
                                {
                                    ExcelImage.AddImage(worksheetPart, filePathImage, imgDesc, 1, rowCheck);
                                    worksheetPart.Worksheet.Save();

                                    //Clear image name
                                    Cell? cell = (Cell)row.ElementAt(0);
                                    if (cell != null)
                                    {
                                        cell.CellValue = new CellValue("");
                                        cell.DataType = new EnumValue<CellValues>(CellValues.String);
                                    }
                                }
                            }
                        }
                    }
                }
                doc.Dispose();
            }

            using (MemoryStream stream = new MemoryStream())
            {
                using (FileStream file = new FileStream(filePathDownload, FileMode.Open, FileAccess.Read))
                {
                    file.CopyTo(stream);
                }
                System.IO.File.Delete(filePathDownload);
                return File(stream.ToArray(), "application/octetstream", fileName);
            }
        } 
    }
}