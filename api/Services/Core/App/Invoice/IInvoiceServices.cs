using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IInvoiceServices
    {
        Task<PagedList<InvoiceResponse>> GetAll(InvoicePagedRequest request);
        Task<InvoiceResponse> GetById(Guid id);
        Task<MemoryStream?> ExportInvoiceById(Guid id);
        Task<int> Create(InvoiceRequest request);
        Task<int> Update(Guid id, InvoiceRequest request);
        Task<int> Delete(Guid id);
        Task<string> GetInvoiceNo(string code);
        Task<StatisticalResponse> GetStatistical(DateTime startDate, DateTime endDate, Guid warehouse_id);
    }
}