using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Entities
{
    public class JobGrade
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Salary { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public byte[] RowVersion { get; set; }

        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();


        public static JobGrade Create(string name, decimal salary)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Job grade name is required.");

            if (salary <= 0)
                throw new ArgumentException(
                    "Job grade salary must be greater than zero.");

            return new JobGrade
            {
                Name = name.Trim(),
                Salary = salary
            };
        }
        public void SetSalary(decimal salary)
        {
            if (salary <= 0)
                throw new ArgumentException("Job grade salary must be greater than zero.");

            Salary = salary;
            UpdatedAt = DateTime.Now;
        }
    }
}
