using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.Reports.EmployeeReport
{
    public class EmployeeReportFilterDto
    {
        public int? EmployeeId { get; set; }

        public string? EmployeeName { get; set; }
        public int? DepartmentId { get; set; }

        public int? JobGradeId { get; set; }

        public int PageNumber { get; set; } = 1;

        [Range(1, 20)]
        public int PageSize { get; set; } 
    }
}
