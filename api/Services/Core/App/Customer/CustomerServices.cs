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
    public class CustomerServices : BaseServices, ICustomerServices
    {
        private readonly IRepository<Customer> customerRepository;
        public CustomerServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            customerRepository = _unitOfWork.GetRepository<Customer>();
        }

        public async Task<PagedList<CustomerResponse>> GetAll(PagedRequest request)
        {
            PagedList<Customer> Customers;
            if(request.get_all)
            {
                Customers = customerRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                Customers = await customerRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<CustomerResponse>>(Customers);
            return dataMapping;
        }

        public async Task<CustomerResponse> GetById(Guid id)
        {
            var Customer = await customerRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<CustomerResponse>(Customer);
            return data;
        }
        public async Task<CustomerResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(CustomerRequest request)
        {
            var Customer = _mapper.Map<Customer>(request);
            await customerRepository.AddAsync(Customer);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, CustomerRequest request)
        {
            var Customer = await _unitOfWork
                        .GetRepository<Customer>()
                        .GetByIdAsync(id);
            if(Customer == null)
            {
                return -1;
            }
            _mapper.Map(request, Customer);
            await customerRepository.UpdateAsync(Customer);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var Customer = await customerRepository.GetByIdAsync(id);
            if(Customer == null)
            {
                return -1;
            }
            await customerRepository.DeleteAsync(Customer);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
