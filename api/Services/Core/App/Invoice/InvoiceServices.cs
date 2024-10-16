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
    public class InvoiceServices : BaseServices, IInvoiceServices
    {
        private readonly IRepository<Invoice> invoiceRepository;
        public InvoiceServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            invoiceRepository = _unitOfWork.GetRepository<Invoice>();
        }

        public async Task<PagedList<InvoiceResponse>> GetAll(PagedRequest request)
        {
            PagedList<Invoice> Invoices;
            if(request.get_all)
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
                         .GetByIdAsync(id);
            var data = _mapper.Map<InvoiceResponse>(Invoice);
            return data;
        }
        public async Task<InvoiceResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(InvoiceRequest request)
        {
            var Invoice = _mapper.Map<Invoice>(request);
            await invoiceRepository.AddAsync(Invoice);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, InvoiceRequest request)
        {
            var Invoice = await _unitOfWork
                        .GetRepository<Invoice>()
                        .GetByIdAsync(id);
            if(Invoice == null)
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
            var Invoice = await invoiceRepository.GetByIdAsync(id);
            if(Invoice == null)
            {
                return -1;
            }
            await invoiceRepository.DeleteAsync(Invoice);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
