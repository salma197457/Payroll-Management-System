using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class JobGradesRepository(PayrollDbContext context) : Repository<JobGrade>(context), IJobGradesRepository
    {


    }
}
