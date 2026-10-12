using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.JobGrades
{
    public class CreateJobGradeDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        public decimal Salary { get; set; }
    }
}
