using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Reports.IncentiveDeductionReport
{
    public class IncentivesDeductionsReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }

        public decimal DepartmentIncentive { get; set; }
        public decimal ExperienceIncentive { get; set; }
        public decimal AttendanceIncentive { get; set; }
        public decimal AbsenceDeduction { get; set; }

        public decimal TotalIncentives { get; set; }
        public decimal TotalDeductions { get; set; }
    }
}
