using AutoMapper;
using Common;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
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
            if (request.get_all)
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
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<ConsigneeResponse>(Consignee);
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
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Consignee == null)
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
            var Consignee = await consigneeRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Consignee == null)
            {
                return -1;
            }
            await consigneeRepository.DeleteAsync(Consignee);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
