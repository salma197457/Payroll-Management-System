using Payroll.Domain.Calculator.CustomEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Entities
{
    public class Payroll
    {
        public int Id { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public decimal BaseSalary { get; set; }

        public decimal DepartmentIncentive { get; set; }

        public decimal ExperienceIncentive { get; set; }

        public decimal AttendanceAdjustment { get; set; }

        public decimal NetSalary { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        public static Payroll Create(int employeeId, int year, int month, PayrollCalculationResult calculation)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid employee.");

            if (year is < 1 or > 9999)
                throw new ArgumentException("Invalid year.");

            if (month is < 1 or > 12)
                throw new ArgumentException("Invalid month.");

            ArgumentNullException.ThrowIfNull(calculation);

            return new Payroll
            {
                EmployeeId = employeeId,
                Year = year,
                Month = month,
                BaseSalary = calculation.BaseSalary,
                DepartmentIncentive = calculation.DepartmentIncentive,
                ExperienceIncentive = calculation.ExperienceIncentive,
                AttendanceAdjustment = calculation.AttendanceAdjustment,
                NetSalary = calculation.NetSalary
            };
        }
    }
}
