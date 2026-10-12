using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.DepartmentIncentives
{
    public class CreateDepartmentIncentiveDto
    {
        [Range(1, int.MaxValue)]
        public int DepartmentId { get; set; }

        [Range(typeof(decimal), "0", "100")]
        public decimal Percentage { get; set; }
    }
}
