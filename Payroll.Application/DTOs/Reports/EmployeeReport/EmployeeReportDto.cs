using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Reports.EmployeeReport
{
    public class EmployeeReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int JobGradeId { get; set; }
        public string JobGradeName { get; set; } 

        public decimal BaseSalary { get; set; }

        public DateOnly JoinDate { get; set; }
    }
}
