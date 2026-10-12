using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Text;

namespace Payroll.Domain.Entities
{
    public class Employee
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; } 
        public string Email { get; set; }
        public DateOnly JoinDate { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public byte[] RowVersion { get; set; }

        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    

        public int DepartmentId { get; set; }
        public Department Department { get; set; }

        public int JobGradeId { get; set; }
        public JobGrade JobGrade { get; set; } 

        public ICollection<EmployeeAbsence> Absences { get; set; }
            = new List<EmployeeAbsence>();

        public ICollection<Payroll> Payrolls { get; set; }
            = new List<Payroll>();



        public static Employee Create(
                    string name,
                    DateOnly birthDate,
                    string address,
                    string phoneNumber,
                    string email,
                    DateOnly joinDate,
                    int departmentId,
                    int jobGradeId)
        {

            Validate(name, birthDate, address, phoneNumber, email, departmentId, jobGradeId);

            if (joinDate > DateOnly.FromDateTime(DateTime.Now))
                throw new ArgumentException("Join date cannot be in the future.");


            return new Employee
            {
                Name = name.Trim(),
                BirthDate = birthDate,
                Address = address.Trim(),
                PhoneNumber = phoneNumber.Trim(),
                Email = email.Trim(),
                JoinDate = joinDate,
                DepartmentId = departmentId,
                JobGradeId = jobGradeId
            };
        }


        public void Update(
            string name,
            DateOnly birthDate,
            string address,
            string phoneNumber,
            string email,
            int departmentId,
            int jobGradeId)
        {

            Validate(name, birthDate, address, phoneNumber, email, departmentId, jobGradeId);

            Name = name.Trim();
            BirthDate = birthDate;
            Address = address.Trim();
            PhoneNumber = phoneNumber.Trim();
            Email = email.Trim();
            DepartmentId = departmentId;
            JobGradeId = jobGradeId;

            UpdatedAt = DateTime.Now;
        }

        private static void Validate(string name,
            DateOnly birthDate,
            string address,
            string phoneNumber,
            string email,
            int departmentId,
            int jobGradeId)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Employee name is required.");

            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Employee address is required.");

            if (string.IsNullOrWhiteSpace(phoneNumber))
                throw new ArgumentException("Employee phone number is required.");

            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Employee email is required.");

            var today = DateOnly.FromDateTime(DateTime.Now);

            if (birthDate == default || birthDate >= today)
                throw new ArgumentException("Invalid birth date.");


            if (departmentId <= 0)
                throw new ArgumentException("Invalid department.");

            if (jobGradeId <= 0)
                throw new ArgumentException("Invalid job grade.");
        }

        public void Delete()
        {
            if (IsDeleted)
                throw new InvalidOperationException("Employee is already deleted.");

            IsDeleted = true;
            UpdatedAt = DateTime.Now;
        }
    }
}
