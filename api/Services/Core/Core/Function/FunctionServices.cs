using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Services.Core.Contracts;
using Services.Common.Repository;
using Services.Core.Interfaces;
using Database.Entities;
using Common;
using Helpers.Auth;
namespace Services.Core.Services
{
    public class FunctionServices : BaseServices, IFunctionServices
    {
        private readonly IRepository<Function> functionRepository;
        public FunctionServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            functionRepository = _unitOfWork.GetRepository<Function>();
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
                         .GetByIdAsync(id);
            var data = _mapper.Map<FunctionResponse>(Function);
            return data;
        }
        public async Task<FunctionResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
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
                        .GetByIdAsync(id);
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
            var Function = await functionRepository.GetByIdAsync(id);
            if(Function == null)
            {
                return -1;
            }
            await functionRepository.DeleteAsync(Function);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
