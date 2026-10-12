using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class EmployeeAbsenceRepository(PayrollDbContext context): Repository<EmployeeAbsence>(context), IEmployeeAbsenceRepository
    {
        public async Task<int> CountAbsencesAsync(int employeeId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default)
        {
            return await DbSet
                .CountAsync(x => x.EmployeeId == employeeId &&
                         x.Date >= startDate &&
                         x.Date <= endDate,cancellationToken);
        }

    }
}
