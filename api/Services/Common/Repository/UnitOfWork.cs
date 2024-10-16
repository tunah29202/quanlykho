using Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Common;

namespace Services.Common.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context; 
        private readonly Dictionary<string, object> _repository;
        private readonly ICurrentUserService _serviceContext;
        public UnitOfWork(AppDbContext context, ICurrentUserService serviceContext)
        {
            _context = context;
            _repository = new Dictionary<string, object>();
            _serviceContext = serviceContext;
        }

        public IRepository<T> GetRepository<T>() where T : class, IEntity
        {
            var type = typeof(T).Name;
            if(!_repository.ContainsKey(type))
            {
                var repository = new Repository<T>(_context, _serviceContext);
                _repository.Add(type, repository);
            }
            return (IRepository<T>)_repository[type];
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public void Dispose()
        {
            _context.Dispose();
        }
    }
}