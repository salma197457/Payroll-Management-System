using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.Reports.IncentiveDeductionReport
{
    public class IncentivesDeductionsReportFilterDto
    {
        [Range(1, 9999)]
        public int? Year { get; set; }

        [Range(1, 12)]
        public int? Month { get; set; }

        public int? EmployeeId { get; set; }

        public int PageNumber { get; set; } = 1;

        [Range(1, 20)]
        public int PageSize { get; set; }
    }
}
