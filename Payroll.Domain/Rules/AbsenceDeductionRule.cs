using Payroll.Domain.Constants;
using Payroll.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Rules
{
    public class AbsenceDeductionRule
    {
        public int Id { get; set; }

        public int MinimumDays { get; set; }

        public decimal Percentage { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] RowVersion { get; set; }


        public AbsenceDeductionRuleType Type { get; set; }


        public static AbsenceDeductionRule Create(int minimumDays, decimal percentage)        
        {
           
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100.");

            if ((!AbsenceRuleConstants.AllowedThresholds.Contains(minimumDays)))
                throw new ArgumentException(
                    "Invalid absence days threshold.");

            var type = minimumDays < AbsenceRuleConstants.IncentiveThreshold
                ? AbsenceDeductionRuleType.Incentive
                : AbsenceDeductionRuleType.Deduction;


            return new AbsenceDeductionRule
            {
                MinimumDays = minimumDays,
                Percentage = percentage,
                Type = type
            };

        }
        public void UpdatePercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100.");

            Percentage = percentage;
            UpdatedAt = DateTime.Now;
        }
    }
}
