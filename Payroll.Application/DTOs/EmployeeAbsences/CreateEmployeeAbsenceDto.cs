using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Payroll.Application.DTOs.EmployeeAbsences
{
    public class CreateEmployeeAbsenceDto
    {
        public int EmployeeId { get; set; }

        public DateOnly Date { get; set; }
    }
}
