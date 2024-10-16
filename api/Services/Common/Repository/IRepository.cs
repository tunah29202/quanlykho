using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace Services.Common.Repository
{
    public interface IRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetQuery(Func<DbSet<T>, IQueryable<T>>? func = null);
        Task<T> GetByIdAsync(Guid id);
        Task AddAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
    }
}