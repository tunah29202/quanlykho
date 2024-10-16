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
    public class PaymentMethodServices : BaseServices, IPaymentMethodServices
    {
        private readonly IRepository<PaymentMethod> paymentMethodRepository;
        public PaymentMethodServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            paymentMethodRepository = _unitOfWork.GetRepository<PaymentMethod>();
        }

        public async Task<PagedList<PaymentMethodResponse>> GetAll(PagedRequest request)
        {
            PagedList<PaymentMethod> PaymentMethods;
            if(request.get_all)
            {
                PaymentMethods = paymentMethodRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                PaymentMethods = await paymentMethodRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<PaymentMethodResponse>>(PaymentMethods);
            return dataMapping;
        }

        public async Task<PaymentMethodResponse> GetById(Guid id)
        {
            var PaymentMethod = await paymentMethodRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<PaymentMethodResponse>(PaymentMethod);
            return data;
        }
        public async Task<PaymentMethodResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(PaymentMethodRequest request)
        {
            var PaymentMethod = _mapper.Map<PaymentMethod>(request);
            await paymentMethodRepository.AddAsync(PaymentMethod);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, PaymentMethodRequest request)
        {
            var PaymentMethod = await _unitOfWork
                        .GetRepository<PaymentMethod>()
                        .GetByIdAsync(id);
            if(PaymentMethod == null)
            {
                return -1;
            }
            _mapper.Map(request, PaymentMethod);
            await paymentMethodRepository.UpdateAsync(PaymentMethod);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var PaymentMethod = await paymentMethodRepository.GetByIdAsync(id);
            if(PaymentMethod == null)
            {
                return -1;
            }
            await paymentMethodRepository.DeleteAsync(PaymentMethod);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
