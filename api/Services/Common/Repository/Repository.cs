using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;

namespace Services.Common.Repository
{
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        protected readonly DbContext _context;
        private readonly DbSet<T> _dbset;
        private readonly ICurrentUserService serviceContext;
        public Repository(DbContext context, ICurrentUserService serviceContext)
        {
            _context = context;
            _dbset = _context.Set<T>();
            this.serviceContext = serviceContext;
        }

        public IQueryable<T> GetQuery(Func<DbSet<T>, IQueryable<T>>? func = null)
        {
            if (func != null)
            {
                return func(_dbset);
            }
            return _dbset;
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _dbset.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            entity.id = Guid.NewGuid();
            entity.created_at = DateTime.UtcNow;
            if (serviceContext.user_id != null)
            {
                entity.created_by = (Guid)serviceContext.user_id;
            }
            await _dbset.AddAsync(entity);
        }

        public async Task UpdateAsync(T entity)
        {
            _context.Attach(entity);
            entity.updated_at = DateTime.UtcNow;

            if (serviceContext.user_id != null)
                entity.updated_by = (Guid)serviceContext.user_id;
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task DeleteAsync(T entity)
        {
            entity.del_flg = true;
            await Task.CompletedTask;
        }
        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

    }
}