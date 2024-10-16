using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.EntityFrameworkCore.Storage;

namespace Services.Common.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<T> GetRepository<T>() where T : class, IEntity;
        Task<int> SaveChangeAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}