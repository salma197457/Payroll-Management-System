using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Application.Interfaces.RepositoryInterfaces
{
    public interface IPayrollRepository : IRepository<Payroll.Domain.Entities.Payroll>
    {
        Task<IReadOnlyList<TResult>> GetByPeriodAsync<TResult>(int year, int month,
            Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default);

        Task<TResult?> GetLatestByEmployeeAsync<TResult>(
            int employeeId,
            Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default);

        Task<IReadOnlyList<TResult>> GetLatestMonthAsync<TResult>(Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default);
    }
}
