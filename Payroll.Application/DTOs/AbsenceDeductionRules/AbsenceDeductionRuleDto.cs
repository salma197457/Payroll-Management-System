using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.AbsenceDeductionRules
{
    public class AbsenceDeductionRuleDto
    {
        public int Id { get; set; }
        public int MinimumDays { get; set; }
        public decimal Percentage { get; set; }
        public string Type { get; set; }
        public byte[] RowVersion { get; set; } 
    }
}
