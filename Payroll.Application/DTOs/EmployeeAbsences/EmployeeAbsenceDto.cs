using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.EmployeeAbsences
{
    public class EmployeeAbsenceDto
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public DateOnly Date { get; set; }
    }
}
