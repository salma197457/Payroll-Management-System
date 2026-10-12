using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Reports.Attendance
{
    public class AttendanceReportDto
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public int WorkingDays { get; set; }
        public int AbsenceDays { get; set; }
        public int AttendanceDays { get; set; }
    }
}
