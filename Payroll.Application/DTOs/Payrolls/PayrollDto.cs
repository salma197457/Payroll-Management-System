using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Payrolls
{
    public class PayrollDto
    {
        public int Id { get; set; }

        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int Year { get; set; }
        public int Month { get; set; }

        public decimal BaseSalary { get; set; }
        public decimal DepartmentIncentive { get; set; }
        public decimal ExperienceIncentive { get; set; }
        public decimal AttendanceAdjustment { get; set; }
        public decimal NetSalary { get; set; }
    }
}
