using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Intrinsics.X86;
using System.Text;

namespace Payroll.Application.DTOs.Employees
{
    public class CreateEmployeeDto
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = null!;

        public DateOnly BirthDate { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; } = null!;

        [Required]
        [Phone]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        public DateOnly JoinDate { get; set; }

        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [Range(1, int.MaxValue)]
        public int JobGradeId { get; set; }
    }
}
