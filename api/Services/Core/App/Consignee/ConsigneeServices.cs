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
    public class ConsigneeServices : BaseServices, IConsigneeServices
    {
        private readonly IRepository<Consignee> consigneeRepository;
        public ConsigneeServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            consigneeRepository = _unitOfWork.GetRepository<Consignee>();
        }

        public async Task<PagedList<ConsigneeResponse>> GetAll(PagedRequest request)
        {
            PagedList<Consignee> Consignees;
            if(request.get_all)
            {
                Consignees = consigneeRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Consignees = await consigneeRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<ConsigneeResponse>>(Consignees);
            return dataMapping;
        }

        public async Task<ConsigneeResponse> GetById(Guid id)
        {
            var Consignee = await consigneeRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<ConsigneeResponse>(Consignee);
            return data;
        }
        public async Task<ConsigneeResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(ConsigneeRequest request)
        {
            var Consignee = _mapper.Map<Consignee>(request);
            await consigneeRepository.AddAsync(Consignee);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, ConsigneeRequest request)
        {
            var Consignee = await _unitOfWork
                        .GetRepository<Consignee>()
                        .GetByIdAsync(id);
            if(Consignee == null)
            {
                return -1;
            }
            _mapper.Map(request, Consignee);
            await consigneeRepository.UpdateAsync(Consignee);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Consignee = await consigneeRepository.GetByIdAsync(id);
            if(Consignee == null)
            {
                return -1;
            }
            await consigneeRepository.DeleteAsync(Consignee);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
