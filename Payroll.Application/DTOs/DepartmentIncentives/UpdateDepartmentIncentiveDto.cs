using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.DepartmentIncentives
{
    public class UpdateDepartmentIncentiveDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Range(typeof(decimal), "0", "100")]
        public decimal Percentage { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; } = null!;
    }
}
