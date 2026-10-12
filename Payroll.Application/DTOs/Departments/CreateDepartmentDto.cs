using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.Departments
{
    public class CreateDepartmentDto
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }
    }
}
