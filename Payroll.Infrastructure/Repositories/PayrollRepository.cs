using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class PayrollRepository(PayrollDbContext context) : Repository<Payroll.Domain.Entities.Payroll>(context), IPayrollRepository
    {
        public async Task<IReadOnlyList<TResult>> GetByPeriodAsync<TResult>(int year,int month,Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.Year == year && x.Month == month)
                .OrderBy(x => x.EmployeeId)
                .Select(selector)
                .ToListAsync(cancellationToken);
        }

        public async Task<TResult?> GetLatestByEmployeeAsync<TResult>(int employeeId, Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .AsNoTracking()
                .Where(x => x.EmployeeId == employeeId)
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(selector)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IReadOnlyList<TResult>> GetLatestMonthAsync<TResult>(Expression<Func<Payroll.Domain.Entities.Payroll, TResult>> selector, CancellationToken cancellationToken = default)
        {
            var latestPeriod = await DbSet
                .AsNoTracking()
                .OrderByDescending(x => x.Year)
                .ThenByDescending(x => x.Month)
                .Select(x => new { x.Year, x.Month })
                .FirstOrDefaultAsync(cancellationToken);

            if (latestPeriod is null)
                return [];

            return await DbSet
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Where(x => x.Year == latestPeriod.Year &&
                            x.Month == latestPeriod.Month)
                .OrderBy(x => x.EmployeeId)
                .Select(selector)
                .ToListAsync(cancellationToken);
        }
    }
}
