using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.ExperienceIncentives
{
    public class ExperienceIncentiveDto
    {
        public int Id { get; set; }
        public int MinimumYears { get; set; }
        public decimal Percentage { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}
