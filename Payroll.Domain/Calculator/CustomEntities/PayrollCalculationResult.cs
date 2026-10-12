using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Calculator.CustomEntities
{
    public sealed record PayrollCalculationResult(
     decimal BaseSalary,
     decimal DepartmentIncentive,
     decimal ExperienceIncentive,
     decimal AttendanceAdjustment,
     decimal NetSalary
 );
}
