using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Application.Interfaces.RepositoryInterfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TResult>>
                   GetAllProjectedAsync<TResult>(Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default, int? pageNumber = null, int? pageSize = null);

        Task<TResult?> GetByIdProjectedAsync<TResult>(int id, Expression<Func<TEntity, TResult>> selector, CancellationToken cancellationToken = default);
        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Remove(TEntity entity);
        //Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> GetForUpdateAsync(int id, byte[] originalRowVersion, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
    }
}
