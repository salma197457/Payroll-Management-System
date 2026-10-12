using Payroll.Domain.Calculator.CustomEntities;
using Payroll.Domain.Constants;
using Payroll.Domain.Enums;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Calculator
{
    public static class PayrollCalculator
    {
        public static PayrollCalculationResult Calculate(PayrollCalculationInput input)
        {
            if (input.BaseSalary <= 0)
                throw new ArgumentException("Base salary must be greater than zero.");

            if (input.AbsenceDays < 0)
                throw new ArgumentException("Absence days cannot be negative.");

            if (input.DepartmentIncentivePercentage is < 0 or > 100 || input.ExperienceIncentivePercentage is < 0 or > 100)
                throw new ArgumentException("Incentive percentages must be between 0 and 100.");

            var departmentIncentive = decimal.Round(
                input.BaseSalary * input.DepartmentIncentivePercentage / 100,2);

            var experienceIncentive = decimal.Round(
                input.BaseSalary * input.ExperienceIncentivePercentage / 100,2);

            var attendanceAdjustment = decimal.Round(
                CalculateAttendanceAdjustment(input.BaseSalary, input.AbsenceDays, input.AbsenceRules),2);


            var netSalary =
                input.BaseSalary +
                departmentIncentive +
                experienceIncentive +
                attendanceAdjustment;

            return new PayrollCalculationResult(input.BaseSalary,departmentIncentive, experienceIncentive, attendanceAdjustment,decimal.Round(netSalary, 2));
        }

        private static decimal CalculateAttendanceAdjustment(decimal baseSalary, int absenceDays, IReadOnlyList<AbsenceDeductionRule> rules)
        {
            if (rules is null)
                throw new ArgumentNullException(nameof(rules));

            if (absenceDays < AbsenceRuleConstants.IncentiveThreshold)
            {
                var incentive = rules.FirstOrDefault(x => x.Type == AbsenceDeductionRuleType.Incentive);

                return incentive is null ? 0 : baseSalary * incentive.Percentage / 100;
            }

            var deduction = rules
                .Where(x => x.Type == AbsenceDeductionRuleType.Deduction && absenceDays >= x.MinimumDays)
                .OrderByDescending(x => x.MinimumDays)
                .FirstOrDefault();

            return deduction is null ? 0 : - baseSalary * (deduction.Percentage / 100);
        }
    }
}
