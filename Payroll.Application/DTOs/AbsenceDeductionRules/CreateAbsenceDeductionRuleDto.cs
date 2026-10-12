using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.AbsenceDeductionRules
{
    public class CreateAbsenceDeductionRuleDto
    {
        public int MinimumDays { get; set; }

        [Range(typeof(decimal), "0", "100")]
        public decimal Percentage { get; set; }
    }
}
