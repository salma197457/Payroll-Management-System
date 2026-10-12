using Microsoft.EntityFrameworkCore;
using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class DepartmentRepository(PayrollDbContext context) : Repository<Department>(context), IDepartmentRepository
    {
   
        //public async Task<bool> ExistsByNameAsync(string name, int? excludeId = null, CancellationToken cancellationToken = default)
        //{
        //    return await DbSet.AnyAsync(d => d.Name == name &&
        //             (!excludeId.HasValue || d.Id != excludeId.Value),
        //             cancellationToken);
        //}

        public async Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken = default)
        {
            return await Context.Employees.IgnoreQueryFilters().AnyAsync(e => e.DepartmentId == departmentId, cancellationToken);
        }

        //public async Task<Department?> GetForUpdateAsync(int id, byte[] originalRowVersion, CancellationToken cancellationToken = default)
        //{
        //    var department = await DbSet.FirstOrDefaultAsync(d => d.Id == id, cancellationToken);

        //    if (department is null)
        //        throw new KeyNotFoundException("Department not found");

        //    Context.Entry(department)
        //        .Property(d => d.RowVersion)
        //        .OriginalValue = originalRowVersion;

        //    return department;
        //}
    }
}
