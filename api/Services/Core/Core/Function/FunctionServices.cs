using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Excel;
using System.Configuration;
using ValidationResult = FluentValidation.Results.ValidationResult;
namespace Services.Core.Services
{
    public class FunctionServices : BaseServices, IFunctionServices
    {
        private readonly IRepository<Function> functionRepository;
        private readonly IWebHostEnvironment env;
        private readonly string _imageStoragePath = "Images";
        private readonly string _Template = "Template";
        public FunctionServices(IUnitOfWork _unitOfWork, IMapper _mapper, IWebHostEnvironment _env) : base(_unitOfWork, _mapper) 
        { 
            functionRepository = _unitOfWork.GetRepository<Function>();
            env = _env;
        }

        public async Task<PagedList<FunctionResponse>> GetAll(PagedRequest request)
        {
            PagedList<Function> data;
            var Functions = await functionRepository.GetQuery()
                                .ExcludeSoftDeleted()
                                .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                .SortBy(request.sort ?? "updated_at.desc")
                                .ToPagedListAsync(request.page, request.size);
            var dataMapping = _mapper.Map<PagedList<FunctionResponse>>(Functions);
            return dataMapping;
        }

        public async Task<FunctionResponse> GetById(Guid id)
        {
            var Function = await functionRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<FunctionResponse>(Function);
            return data;
        }
        public async Task<(object?, MemoryStream?)> ImportExcel(IFormFile file)
        {
            var directory = Path.Combine(env.WebRootPath, _imageStoragePath);
            if (!Directory.Exists(_imageStoragePath))
            {
                Directory.CreateDirectory(directory);
            };
            const int totalCol = 5;
            (List<FunctionRequest> lstFunction, List<string> messages) = await ExcelImport.getDataFromExcel<FunctionRequest>(file, totalCol, directory);
            Dictionary<int, List<string>> lstErrors = new Dictionary<int, List<string>>();
            FunctionRequestValidator validator = new FunctionRequestValidator();
            for (int i = 0; i < lstFunction.Count; i++)
            {
                ValidationResult results = validator.Validate(lstFunction[i]);
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
                var result = await ExcelImport.getFileError(file, lstErrors, ExcelImport.ColumnIndexToLetter(totalCol + 1));
                if (result.Item1 != null)
                    return (result.Item1, null);
                return (null, result.Item2);
            }
            foreach (var item in lstFunction)
            {
                await Create(item);
            };
            return (new BaseResponse(ResponseCode.Success, "Import Success"), null);
        }
        public async Task<int> Create(FunctionRequest request)
        {
            var Function = _mapper.Map<Function>(request);
            await functionRepository.AddAsync(Function);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, FunctionRequest request)
        {
            var Function = await _unitOfWork
                                    .GetRepository<Function>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Function == null)
            {
                return -1;
            }
            _mapper.Map(request, Function);
            await functionRepository.UpdateAsync(Function);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Function = await functionRepository           
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if(Function == null)
            {
                return -1;
            }
            await functionRepository.DeleteAsync(Function);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
        public async Task<List<FunctionResponse>> GetAsTreeView()
        {
            List<FunctionResponse> response = new List<FunctionResponse>();
            var listFunction = functionRepository.GetQuery().ExcludeSoftDeleted().ToList();
            var listParent = listFunction.Where(x => x.parent_cd == null).Distinct().ToList();
            response.AddRange(_mapper.Map<List<FunctionResponse>>(listParent));
            foreach(var item in response)
            {
                var listChildren = listFunction.Where(x => x.parent_cd == item.code).ToArray();
                if(listChildren!=null && listChildren.Length > 0)
                {
                    item.items = new List<FunctionResponse>();
                    item.items.AddRange(_mapper.Map<List<FunctionResponse>>(listChildren));
                    foreach(var children in item.items)
                    {
                        var listApi = listFunction.Where(x => x.parent_cd == children.code).ToArray();
                        if(listApi != null && listApi.Length > 0)
                        {
                            children.items = new List<FunctionResponse>();
                            children.items.AddRange(_mapper.Map<List<FunctionResponse>>(listApi));
                        }
                    }    
                }
            }
            return response;
        } 
    }
}
