using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.RepositoryInterfaces
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        //Task<bool> ExistsByNameAsync(string name,int? excludeId = null, CancellationToken cancellationToken = default);

        Task<bool> HasActiveEmployeesAsync(int departmentId, CancellationToken cancellationToken = default);

        //Task<Department?> GetForUpdateAsync(int id, byte[] originalRowVersion, CancellationToken cancellationToken = default);
    }
}

