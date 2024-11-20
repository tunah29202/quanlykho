using AutoMapper;
using Common;
using Database.Entities;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Helpers.Excel;
using Microsoft.EntityFrameworkCore;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
namespace Services.Core.Services
{
    public class InvoiceServices : BaseServices, IInvoiceServices
    {
        private readonly IRepository<Invoice> invoiceRepository;
        private readonly IWebHostEnvironment env;
        public InvoiceServices(IUnitOfWork _unitOfWork, IMapper _mapper, IWebHostEnvironment _env) : base(_unitOfWork, _mapper)
        {
            invoiceRepository = _unitOfWork.GetRepository<Invoice>();
            env = _env;

        }

        public async Task<PagedList<InvoiceResponse>> GetAll(PagedRequest request)
        {
            PagedList<Invoice> Invoices;
            if (request.get_all)
            {
                Invoices = invoiceRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Invoices = await invoiceRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.invoice_no.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<InvoiceResponse>>(Invoices);
            return dataMapping;
        }

        public async Task<InvoiceResponse> GetById(Guid id)
        {
            var Invoice = await invoiceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<InvoiceResponse>(Invoice);
            return data;
        }

        public async Task<MemoryStream?> ExportInvoiceById(Guid id)
        {
            var fileName = $"Invoice_{DateTimeExtention.ToDateTimeStampString(DateTime.Now)}.xlsx";
            string templatePath = Path.Combine(env.WebRootPath, "Template", InvoiceExcel.FILE_NAME);
            string fileTemplate = Path.GetFullPath(templatePath);
            if(!File.Exists(fileTemplate))
                return null;
            string template = Path.Combine(Path.GetDirectoryName(fileTemplate), fileName);
            File.Copy(fileTemplate, template, true);
            var invoice = invoiceRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .FilterById(id)
                            .Include(s => s.shipper)
                            .Include(w => w.warehouse)
                            .Include(c => c.carton)
                                .ThenInclude(cd => cd.carton_details)
                                    .ThenInclude(p => p.product)
                                        .ThenInclude(c => c.category)
                                            .ThenInclude(i => i.ingredients)
                            .Include(c => c.carton.customer)
                            .Include(o => o.order)
                            .Include(p => p.payment_method)
                            .FirstOrDefault();
            if(invoice == null)
                return null;
            WriteToExcel(template, invoice);

            var data = _mapper.Map<InvoiceResponse>(invoice);

            using (var ms = new MemoryStream(File.ReadAllBytes(template)))
            {
                using (FileStream file = new FileStream(template, FileMode.Open))
                {
                    await file.CopyToAsync(ms);
                }
                File.Delete(template);
                invoice.status = 1;
                await _unitOfWork.SaveChangeAsync();
                return ms;
            }
        }

        private void WriteToExcel(string template, Invoice invoice)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(template, true))
            {
                WorkbookPart? workbookPart = document.WorkbookPart;
                // Fill Sheet Carton
                #region Fill Sheet Carton
                Sheet? sheetCarton = workbookPart?.Workbook?
                                                    .GetFirstChild<Sheets>()?
                                                    .Elements<Sheet>()
                                                    .FirstOrDefault(x => x.Name == InvoiceExcel.SHEET_CARTON);
                if(sheetCarton == null)
                {
                    return;
                }

                var relationshipId = sheetCarton?.Id?.Value;
                WorksheetPart? worksheetPart = (WorksheetPart?)document?.WorkbookPart?.GetPartById(relationshipId);

                //Get reference to SheetData
                SheetData? sheetData = worksheetPart?.Worksheet?.GetFirstChild<SheetData>();
                int currrentRowIndex = InvoiceExcel.CARTON_COLUMN_START_INDEX; 
                List<Product> products = new List<Product>();
                Carton carton = invoice.carton;
                FillCarton(worksheetPart, carton, sheetData, currrentRowIndex);
                #endregion
                //Fill Sheet Invoice
                #region Fill Sheet Invoice
                Sheet? sheetInvoice = workbookPart?.Workbook?
                                                        .GetFirstChild<Sheets>()?
                                                        .Elements<Sheet>()
                                                        .FirstOrDefault(x => x.Name == InvoiceExcel.SHEET_INVOICE);
                if(sheetInvoice == null)
                    return;
                
                relationshipId = sheetInvoice?.Id?.Value;
                worksheetPart = (WorksheetPart?)document?.WorkbookPart?.GetPartById(relationshipId);
                //Get reference to SheetData
                sheetData = worksheetPart?.Worksheet?.GetFirstChild<SheetData>();
                int productRowIndex = FillProductInInvoice(sheetData, carton, workbookPart);
                FillInvoiceHeader(sheetData, invoice, workbookPart, productRowIndex);
                #endregion
                
                //Get or create the CalculationProperties element
                CalculationProperties? calc = workbookPart?.Workbook?.CalculationProperties;
                if(calc == null)
                {
                    calc = new CalculationProperties();
                    workbookPart?.Workbook.Append(calc);
                } 
                // Set the CalculationOnSave value
                calc.ForceFullCalculation = true;
                calc.FullCalculationOnLoad = true;
            }
        }
        private int FillProductInInvoice(SheetData? sheetData, Carton carton, WorkbookPart workbookPart)
        {
            int itemNo = 1;
            int productRowIndex = 16;
            foreach(var cartonDetail in carton.carton_details)
            {
                Product product = cartonDetail.product;
                if(itemNo > 1)
                {
                    ExcelExport.InsertCopyRow(productRowIndex - 1, sheetData);
                }
                ExcelExport.WriteCellValue(productRowIndex, 1, sheetData, itemNo.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 2, sheetData, product.code);
                ExcelExport.WriteCellValue(productRowIndex, 3, sheetData, product.name);
                ExcelExport.WriteCellValue(productRowIndex, 4, sheetData, product.origin);
                ExcelExport.WriteCellValue(productRowIndex, 5, sheetData, product.price_unit.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 6, sheetData, cartonDetail.unit.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 7, sheetData, cartonDetail.quantity.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 8, sheetData, null, $"=E{productRowIndex}*G{productRowIndex}", true, false, workbookPart);

                productRowIndex++;
                itemNo++;
            }
            return productRowIndex;
        }
        
        private void FillInvoiceHeader(SheetData? sheetData, Invoice invoice, WorkbookPart workbookPart, int productEndRowIndex)
        {
            const int SHIPPER_CUSTOMER_COL_INDEX = 2; 
            const int INVOICE_COL_INDEX = 7; 
            const int productStartRowIndex = 16; 
            if(sheetData == null)
                return;
            //Fill Shipper Info
            ExcelExport.WriteCellValue(4, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice.shipper.name);
            ExcelExport.WriteCellValue(5, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice.shipper.address);
            ExcelExport.WriteCellValue(6, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice.shipper.email);
            ExcelExport.WriteCellValue(7, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice.shipper.tel);
            //Fill Customer Info
            ExcelExport.WriteCellValue(9, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.name);
            ExcelExport.WriteCellValue(10, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.company_name);
            ExcelExport.WriteCellValue(11, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.address);
            ExcelExport.WriteCellValue(12, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.tax);
            ExcelExport.WriteCellValue(13, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.email);
            ExcelExport.WriteCellValue(14, SHIPPER_CUSTOMER_COL_INDEX, sheetData, invoice?.carton?.customer?.tel);
            //Fill Invoice Info
            ExcelExport.WriteCellValue(3, INVOICE_COL_INDEX, sheetData, invoice.order.order_no);
            ExcelExport.WriteCellValue(4, INVOICE_COL_INDEX, sheetData, invoice.invoice_no);
            ExcelExport.WriteCellValue(5, INVOICE_COL_INDEX, sheetData, invoice.invoice_date?.ToString("yyyy/MM/dd"));
            ExcelExport.WriteCellValue(6, INVOICE_COL_INDEX, sheetData, invoice.shipped_per);
            ExcelExport.WriteCellValue(7, INVOICE_COL_INDEX, sheetData, invoice.shipped_date?.ToString("yyyy/MM/dd"));
            ExcelExport.WriteCellValue(8, INVOICE_COL_INDEX, sheetData, invoice.total_weight);
            ExcelExport.WriteCellValue(9, INVOICE_COL_INDEX, sheetData, invoice.total_volumn.ToString());
            ExcelExport.WriteCellValue(10, INVOICE_COL_INDEX, sheetData, null, $"=SUM(H{productStartRowIndex}:H{productEndRowIndex})", true, false, workbookPart);
            ExcelExport.WriteCellValue(11, INVOICE_COL_INDEX, sheetData, null, $"=10%*SUM(H{productStartRowIndex}:H{productEndRowIndex})", true, false, workbookPart);
            ExcelExport.WriteCellValue(12, INVOICE_COL_INDEX, sheetData, null, $"=(1+10%)*SUM(H{productStartRowIndex}:H{productEndRowIndex})", true, false, workbookPart);
            ExcelExport.WriteCellValue(13, INVOICE_COL_INDEX, sheetData, invoice.payment_method.name);
            ExcelExport.WriteCellValue(14, INVOICE_COL_INDEX, sheetData, invoice.notes);
        }

        private void FillCarton(WorksheetPart? worksheetPart, 
                                Carton carton, 
                                SheetData sheetData, 
                                int currrentRowIndex)
        {
            //carton_no
            ExcelExport.WriteCellValue(currrentRowIndex, 1, sheetData, carton.carton_no);  
            //customer code
            ExcelExport.WriteCellValue(currrentRowIndex, 2, sheetData, carton.customer.code);            
            //net_weight
            ExcelExport.WriteCellValue(currrentRowIndex, 3, sheetData, carton.net_weight.ToString());
            //gross_weight
            ExcelExport.WriteCellValue(currrentRowIndex, 4, sheetData, carton.gross_weight.ToString());
            //height
            ExcelExport.WriteCellValue(currrentRowIndex, 5, sheetData, carton.height.ToString());

            ExcelExport.WriteCellValue(currrentRowIndex, 6, sheetData, carton.width.ToString());
            //length
            ExcelExport.WriteCellValue(currrentRowIndex, 7, sheetData, carton.length.ToString());
            //volume
            ExcelExport.WriteCellValue(currrentRowIndex, 8, sheetData, carton.volume.ToString());
            //total_amount
            ExcelExport.WriteCellValue(currrentRowIndex, 9, sheetData, carton.total_amount.ToString());

            int productRowIndex = currrentRowIndex + 3;
            int itemNo = 1;
            foreach(var cartonDetail in carton.carton_details)
            {
                Product product = cartonDetail.product;
                if(itemNo > 1)
                {
                    ExcelExport.InsertCopyRow(productRowIndex -1, sheetData);
                }
                ExcelExport.WriteCellValue(productRowIndex, 1, sheetData, itemNo.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 3, sheetData, product.code);
                ExcelExport.WriteCellValue(productRowIndex, 4, sheetData, product.name);
                ExcelExport.WriteCellValue(productRowIndex, 5, sheetData, product.origin);
                ExcelExport.WriteCellValue(productRowIndex, 6, sheetData, product.price_unit.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 7, sheetData, cartonDetail.quantity.ToString());
                ExcelExport.WriteCellValue(productRowIndex, 8, sheetData, product.category.name);
                ExcelExport.WriteCellValue(productRowIndex, 9, sheetData, product.ingredient);
                //export image to excel
                Row? row = sheetData.Elements<Row>().Where(r => r.RowIndex == productRowIndex).FirstOrDefault();
                if(row != null)
                {
                    if(!string.IsNullOrEmpty(product.image))
                    {
                        var filePathImage = Path.Combine(env.WebRootPath, "Images", product.image);
                        var fileName = Path.GetFileNameWithoutExtension(product.image);
                        if(File.Exists(filePathImage))
                        {
                            row.CustomHeight = true;
                            row.Height = new DoubleValue((double)130);
                            ExcelImage.AddImage(worksheetPart, filePathImage, fileName, 2, productRowIndex);
                        }
                        else
                        {
                            row.CustomHeight = true;
                            row.Height = new DoubleValue((double)20);
                        }
                    }
                }
                productRowIndex++;
                itemNo++;
            }
        }
        

        public async Task<int> Create(InvoiceRequest request)
        {
            var checkInvoice = await invoiceRepository.GetQuery()
                                        .FirstOrDefaultAsync(i => i.carton_id == request.carton_id);
            if (checkInvoice != null)
            {
                _mapper.Map(request, checkInvoice);
                checkInvoice.del_flg = false;
                await invoiceRepository.UpdateAsync(checkInvoice);
            }
            else
            {
                var Invoice = _mapper.Map<Invoice>(request);
                await invoiceRepository.AddAsync(Invoice);
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, InvoiceRequest request)
        {
            var Invoice = await _unitOfWork
                                    .GetRepository<Invoice>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Invoice == null)
            {
                return -1;
            }
            _mapper.Map(request, Invoice);
            await invoiceRepository.UpdateAsync(Invoice);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Invoice = await invoiceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Invoice == null)
            {
                return -1;
            }
            await invoiceRepository.DeleteAsync(Invoice);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
