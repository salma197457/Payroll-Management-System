using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Rules
{
    public class DepartmentIncentive
    {
        public int Id { get; set; }
        public decimal Percentage { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] RowVersion { get; set; }


        public int DepartmentId { get; set; }
        public Department Department { get; set; }


        public static DepartmentIncentive Create(int departmentId, decimal percentage)
        {
            if (departmentId <= 0)
                throw new ArgumentException("Invalid department.");

            if (percentage < 0 || percentage > 100)
                throw new ArgumentException(
                    "Incentive percentage must be between 0 and 100.");

            return new DepartmentIncentive
            {
                DepartmentId = departmentId,
                Percentage = percentage
            };
        }

        public void UpdatePercentage(decimal percentage)
        {
            if (percentage < 0 || percentage > 100)
                throw new ArgumentException("Incentive percentage must be between 0 and 100.");

            Percentage = percentage;
            UpdatedAt = DateTime.Now;
        }
    }
}
