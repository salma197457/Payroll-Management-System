using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Rules
{
    public class ExperienceIncentive
    {
        public int Id { get; set; }

        public int MinimumYears { get; set; }

        public decimal Percentage { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] RowVersion { get; set; }


        public static ExperienceIncentive Create(int minimumYears, decimal percentage)
        {
            if (minimumYears < 0)
                throw new ArgumentException("Minimum years cannot be negative.");

            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100.");

            return new ExperienceIncentive
            {
                MinimumYears = minimumYears,
                Percentage = percentage
            };
        }

        public void UpdatePercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Percentage must be between 0 and 100");

            Percentage = percentage;
            UpdatedAt = DateTime.Now;
        }

    }
}
