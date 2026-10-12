using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Application.DTOs.Employees
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string Name { get; set; } 
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public DateOnly JoinDate { get; set; }

        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } 

        public int JobGradeId { get; set; }
        public string JobGradeName { get; set; } 
        public decimal BaseSalary { get; set; }

        public byte[] RowVersion { get; set; } 
    }
}
