using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Reports.IncentiveDeductionReport
{
    public class IncentivesDeductionsReportResultDto 
    {
        public IReadOnlyList<IncentivesDeductionsReportDto> Items { get; set; } = [];

        public int TotalCount { get; set; }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalPages => PageSize > 0 ? (int)Math.Ceiling((double)TotalCount / PageSize) : 0;

        public decimal TotalIncentives { get; set; }

        public decimal TotalDeductions { get; set; }
    }
}
