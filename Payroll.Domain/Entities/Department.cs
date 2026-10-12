using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte[] RowVersion { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }



        public ICollection<Employee> Employees { get; set; }
            = new List<Employee>();

        public DepartmentIncentive Incentive { get; set; }


        public static Department Create(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name is required.");

            return new Department
            {
                Name = name.Trim()
            };
        }

        public void Rename(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name is required.");

            Name = name.Trim();
            UpdatedAt = DateTime.Now;
        }

        public void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Department is already deleted.");

            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
