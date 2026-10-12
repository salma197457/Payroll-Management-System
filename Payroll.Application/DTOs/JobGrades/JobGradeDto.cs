using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.JobGrades
{
    public class JobGradeDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public byte[] RowVersion { get; set; }
    }
}
