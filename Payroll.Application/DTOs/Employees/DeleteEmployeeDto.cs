using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs
{
    public class DeleteEmployeeDto
    {
        [Range(1, int.MaxValue)]
        public int Id { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(8)]
        public byte[] RowVersion { get; set; } = null!;
    }
}
