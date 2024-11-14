using AutoMapper;
using Common;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Common.Repository;
using Services.Core.Contracts;
using Services.Core.Interfaces;
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
            if (request.get_all)
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
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .Include(x => x.carton_details)
                                    .FirstOrDefaultAsync();
            var data = _mapper.Map<InvoiceResponse>(Invoice);
            return data;
        }

        public async Task<int> Create(InvoiceRequest request)
        {
            var checkInvoice = await invoiceRepository.GetQuery()
                                        .FirstOrDefaultAsync(i => i.carton_id == request.carton_id);
            if (checkInvoice != null)
            {
                _mapper.Map(request, checkInvoice);
                checkInvoice.del_flg = false;
                await invoiceRepository.UpdateAsync(checkInvoice);
            }
            else
            {
                var Invoice = _mapper.Map<Invoice>(request);
                await invoiceRepository.AddAsync(Invoice);
            }
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, InvoiceRequest request)
        {
            var Invoice = await _unitOfWork
                                    .GetRepository<Invoice>()
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Invoice == null)
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
            var Invoice = await invoiceRepository
                                    .GetQuery()
                                    .ExcludeSoftDeleted()
                                    .FilterById(id)
                                    .FirstOrDefaultAsync();
            if (Invoice == null)
            {
                return -1;
            }
            await invoiceRepository.DeleteAsync(Invoice);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
