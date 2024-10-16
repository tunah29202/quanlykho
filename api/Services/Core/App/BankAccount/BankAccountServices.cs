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
    public class BankAccountServices : BaseServices, IBankAccountServices
    {
        private readonly IRepository<BankAccount> bankAccountRepository;
        public BankAccountServices(IUnitOfWork _unitOfWork, IMapper _mapper) : base(_unitOfWork, _mapper) 
        { 
            bankAccountRepository = _unitOfWork.GetRepository<BankAccount>();
        }

        public async Task<PagedList<BankAccountResponse>> GetAll(PagedRequest request)
        {
            PagedList<BankAccount> BankAccounts;
            if(request.get_all)
            {
                BankAccounts = bankAccountRepository
                            .GetQuery()
                            .ExcludeSoftDeleted()
                            .SortBy(request.sort ?? "updated_at.desc")
                            .ToAllPageList();
            }
            else
            {
                BankAccounts = await bankAccountRepository.GetQuery()
                                    .ExcludeSoftDeleted()
                                    .Where(x => !string.IsNullOrEmpty(request.search) ? x.card_name.ToLower().Contains(request.search.ToLower()) : true)
                                    .SortBy(request.sort ?? "updated_at.desc")
                                    .ToPagedListAsync(request.page, request.size);
            }
            var dataMapping = _mapper.Map<PagedList<BankAccountResponse>>(BankAccounts);
            return dataMapping;
        }

        public async Task<BankAccountResponse> GetById(Guid id)
        {
            var BankAccount = await bankAccountRepository
                         .GetByIdAsync(id);
            var data = _mapper.Map<BankAccountResponse>(BankAccount);
            return data;
        }
        public async Task<BankAccountResponse> GetInfoLoginById(Guid id)
        {
            var data = await GetById(id);
            return data;
        }

        public async Task<int> Create(BankAccountRequest request)
        {
            var BankAccount = _mapper.Map<BankAccount>(request);
            await bankAccountRepository.AddAsync(BankAccount);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Update(Guid id, BankAccountRequest request)
        {
            var BankAccount = await _unitOfWork
                        .GetRepository<BankAccount>()
                        .GetByIdAsync(id);
            if(BankAccount == null)
            {
                return -1;
            }
            _mapper.Map(request, BankAccount);
            await bankAccountRepository.UpdateAsync(BankAccount);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }

        public async Task<int> Delete(Guid id)
        {
            var BankAccount = await bankAccountRepository.GetByIdAsync(id);
            if(BankAccount == null)
            {
                return -1;
            }
            await bankAccountRepository.DeleteAsync(BankAccount);
            var count = await _unitOfWork.SaveChangeAsync();
            return count;
        }
    }
}
