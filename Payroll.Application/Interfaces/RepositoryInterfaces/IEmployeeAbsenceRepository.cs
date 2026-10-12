using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.Interfaces.RepositoryInterfaces
{
    public interface IEmployeeAbsenceRepository : IRepository<EmployeeAbsence>
    {
        Task<int> CountAbsencesAsync(int employeeId, DateOnly startDate, DateOnly endDate, CancellationToken cancellationToken = default);
    }
}
