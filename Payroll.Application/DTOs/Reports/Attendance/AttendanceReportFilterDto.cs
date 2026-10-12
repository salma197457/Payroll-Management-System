using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.Reports.Attendance
{
    public class AttendanceReportFilterDto
    {
        [Required]
        public DateOnly? FromDate { get; set; }

        [Required]
        public DateOnly? ToDate { get; set; }

        public int? EmployeeId { get; set; }

        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }
}
