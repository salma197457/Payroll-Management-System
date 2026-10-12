using Payroll.Application.Interfaces.RepositoryInterfaces;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Repositories
{
    public class AbsenceDeductionRuleRepository(PayrollDbContext context) : Repository<AbsenceDeductionRule>(context), IAbsenceDeductionRuleRepository
    {
    }
}
