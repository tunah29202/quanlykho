using Common;
using Services.Core.Contracts;
namespace Services.Core.Interfaces
{
    public interface IResourceServices
    {
        Task<PagedList<ResourceResponse>> GetAll(PagedRequest request);
        List<ResourceResponse> GetByScreen(string lang, string module, string screen);
        Task<ResourceResponse> GetById(Guid id);
        Task<int> Create(ResourceRequest request);
        Task<int> Update(Guid id, ResourceRequest request);
        Task<int> Delete(Guid id);
        Task<(object?, MemoryStream?)> ImportExcel(ResourceImportRequest request);
        Task<PagedList<string>?> GetScreenByModule(ResourcePagedRequest request);
    }
}