using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Calculator.CustomEntities
{
    public sealed record PayrollCalculationInput(
     decimal BaseSalary,
     decimal DepartmentIncentivePercentage,
     decimal ExperienceIncentivePercentage,
     int AbsenceDays,
     IReadOnlyList<AbsenceDeductionRule> AbsenceRules
 );
}
