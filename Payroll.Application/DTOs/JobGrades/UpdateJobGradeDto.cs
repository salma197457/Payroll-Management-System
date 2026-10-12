using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.JobGrades
{
    public class UpdateJobGradeDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public decimal Salary { get; set; }

        [Required]
        public byte[] RowVersion { get; set; }

    }
}
