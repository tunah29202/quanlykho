using AutoMapper;
using Common;
using Database.Entities;
using FluentValidation;
using Helpers.Excel;
using Microsoft.EntityFrameworkCore;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace Services.Core.Services
{
    public class ResourceServices : BaseServices, IResourceServices
    {
        private readonly IRepository<Resource> resourceRepository;
        private readonly IWebHostEnvironment env;
        private readonly string _imageStoragePath = "Images";
        private readonly string _Template = "Template";
        public ResourceServices(IUnitOfWork _unitOfWork, IMapper _mapper, IWebHostEnvironment _env) : base(_unitOfWork, _mapper)
        {
            env = _env;
            resourceRepository = _unitOfWork.GetRepository<Resource>();
        }

        public async Task<PagedList<ResourceResponse>> GetAll(PagedRequest request)
        {
            PagedList<Resource> data;
            var resources = await resourceRepository.GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => !string.IsNullOrEmpty(request.search) ? x.screen.ToLower().Contains(request.search.ToLower()) : true)
                                .SortBy(request.sort ?? "updated_at.desc")
                                .ToPagedListAsync(request.page, request.size);
            var dataMapping = _mapper.Map<PagedList<ResourceResponse>>(resources);
            return dataMapping;
        }

        public List<ResourceResponse> GetByScreen(string lang, string module, string screen)
        {
            var data = resourceRepository
                                .GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => x.lang == lang)
                                .Where(x => x.module == module)
                                .Where(x => x.screen == screen)
                                .ToList();
            var dataMapping = _mapper.Map<List<ResourceResponse>>(data);
            return dataMapping;
        }

        public async Task<ResourceResponse> GetById(Guid id)
        {
            var resource = await resourceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<ResourceResponse>(resource);
            return data;
        }

        public async Task<(object?, MemoryStream?)> ImportExcel(ResourceImportRequest request)
        {
            var directory = Path.Combine(env.WebRootPath, _imageStoragePath);
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(directory);
            };
            const int totalCol = 5;
            (List<ResourceRequest> lstResource, List<string> messages) = await ExcelImport.getDataFromExcel<ResourceRequest>(request.file, totalCol, directory);
            Dictionary<int, List<string>> lstErrors = new Dictionary<int, List<string>>();
            ResourceRequestValidator validator = new ResourceRequestValidator();
            for (int i = 0; i < lstResource.Count; i++)
            {
                ValidationResult results = validator.Validate(lstResource[i]);
                if (!results.IsValid)
                {
                    foreach (var error in results.Errors)
                    {
                        ExcelImport.addError(lstErrors, i, error.ErrorMessage);
                    }
                }
            }
            if (messages.Count > 0)
            {
                return (new BaseResponse(ResponseCode.Invalid, "Duplicate Errors"), null);
            }
            if (lstErrors.Count > 0)
            {
                var result = await ExcelImport.getFileError(request.file, lstErrors, ExcelImport.ColumnIndexToLetter(totalCol + 1));
                if (result.Item1 != null)
                    return (result.Item1, null);
                return (null, result.Item2);
            }
            foreach (var item in lstResource)
            {
                await CreateNewResource(item);
            };
            return (new BaseResponse(ResponseCode.Success, "Import Success"), null);
        }

        public async Task<int> CreateNewResource(ResourceRequest request)
        {
            var resource = _mapper.Map<Resource>(request);
            await resourceRepository.AddAsync(resource);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Create(ResourceRequest request)
        {
            var resource = _mapper.Map<Resource>(request);
            await resourceRepository.AddAsync(resource);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, ResourceRequest request)
        {
            var resource = await _unitOfWork
                                    .GetRepository<Resource>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (resource == null)
            {
                return -1;
            }
            _mapper.Map(request, resource);
            await resourceRepository.UpdateAsync(resource);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var resource = await resourceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (resource == null)
            {
                return -1;
            }
            await resourceRepository.DeleteAsync(resource);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
        public async Task<PagedList<string>?> GetScreenByModule(ResourcePagedRequest request)
        {
            return resourceRepository.GetQuery()
                        .ExcludeSoftDeleted()
                        .Where(x => x.module.Equals(request.module))
                        .Select(e => e.screen)
                        .Distinct()
                        .SortBy(request.sort)
                        .ToAllPageList();
        }
    }
}
