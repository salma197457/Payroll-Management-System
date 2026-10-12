using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class EmployeeRepository(PayrollDbContext context) : Repository<Employee>(context), IEmployeeRepository
    {
    }
}
