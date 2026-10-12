using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Departments
{
    public class DepartmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
