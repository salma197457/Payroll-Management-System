using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Entities
{
    public class EmployeeAbsence
    {
        public int Id { get; set; }

        public DateOnly Date { get; set; }

        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        public static EmployeeAbsence Create(int employeeId, DateOnly date)
        {
            if (employeeId <= 0)
                throw new ArgumentException("Invalid employee.");

            if (date > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Absence date cannot be in the future.");

            return new EmployeeAbsence
            {
                EmployeeId = employeeId,
                Date = date
            };
        }
    }
}
    