using Microsoft.EntityFrameworkCore;
using Payroll.Domain.Entities;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure
{
    public class PayrollDbContext : DbContext
    {
        public PayrollDbContext(
        DbContextOptions<PayrollDbContext> options)
        : base(options)
        {
        }
        public DbSet<Employee> Employees => Set<Employee>();

        public DbSet<Department> Departments => Set<Department>();

        public DbSet<JobGrade> JobGrades => Set<JobGrade>();

        public DbSet<DepartmentIncentive> DepartmentIncentives
            => Set<DepartmentIncentive>();

        public DbSet<ExperienceIncentive> ExperienceIncentives
            => Set<ExperienceIncentive>();

        public DbSet<EmployeeAbsence> EmployeeAbsences
            => Set<EmployeeAbsence>();

        public DbSet<AbsenceDeductionRule> AbsenceDeductionRules
            => Set<AbsenceDeductionRule>();

        public DbSet<Payroll.Domain.Entities.Payroll> Payrolls => Set<Payroll.Domain.Entities.Payroll>();
        protected override void OnModelCreating(
        ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(PayrollDbContext).Assembly);
        }

    }
}
