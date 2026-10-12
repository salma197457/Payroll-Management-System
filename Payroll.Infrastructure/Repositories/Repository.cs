using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly PayrollDbContext Context;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(PayrollDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }
        public virtual async Task<IReadOnlyList<TResult>> 
            GetAllProjectedAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default, int? pageNumber = null, int? pageSize = null)
        {
            //return await DbSet.AsNoTracking().Select(selector).ToListAsync(cancellationToken);
            var query = DbSet.AsNoTracking().AsQueryable();

            if (pageNumber.HasValue && pageSize.HasValue)
            {
                query = query.OrderBy(e => EF.Property<int>(e, "Id")).Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }

            return await query.Select(selector).ToListAsync(cancellationToken);
        }
        public virtual async Task<TResult?> GetByIdProjectedAsync<TResult>(int id, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(e => EF.Property<int>(e, "Id") == id)
                .Select(selector)
                .FirstOrDefaultAsync(cancellationToken);
        }
        public virtual async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await DbSet.AddAsync(entity, cancellationToken);
        }

        public virtual void Remove(TEntity entity)
        {
            DbSet.Remove(entity);
        }
        public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await DbSet.AnyAsync(predicate, cancellationToken);
        }
        public async Task<TEntity> GetForUpdateAsync(int id, byte[] originalRowVersion, CancellationToken cancellationToken = default)
        {
            var entity = await DbSet.FirstOrDefaultAsync(e => EF.Property<int>(e, "Id") == id, cancellationToken);

            if (entity is null)
                throw new KeyNotFoundException($"{typeof(TEntity).Name} not found.");

            Context.Entry(entity).Property("RowVersion").OriginalValue = originalRowVersion;

            return entity;
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbSet.AsNoTracking().ToListAsync(cancellationToken);
        }
        public virtual Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return DbSet.AsNoTracking().CountAsync(cancellationToken);
        }
    }
}
