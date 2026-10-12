using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.DepartmentIncentives
{
    public class DepartmentIncentiveDto
    {

        public int Id { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = null!;
        public decimal Percentage { get; set; }
        public byte[] RowVersion { get; set; } = null!;
    }
}

