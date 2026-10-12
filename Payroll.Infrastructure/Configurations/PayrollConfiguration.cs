using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class PayrollConfiguration : IEntityTypeConfiguration<Payroll.Domain.Entities.Payroll>
    {
        public void Configure(EntityTypeBuilder<Payroll.Domain.Entities.Payroll> builder)
        {
            builder.ToTable("Payrolls");

            builder.HasKey(p => p.Id);

            builder.Property(p => p.BaseSalary)
                .HasPrecision(18, 2);

            builder.Property(p => p.DepartmentIncentive)
                .HasPrecision(18, 2);

            builder.Property(p => p.ExperienceIncentive)
                .HasPrecision(18, 2);

            builder.Property(p => p.AttendanceAdjustment)
                .HasPrecision(18, 2);

            builder.Property(p => p.NetSalary)
                .HasPrecision(18, 2);

            builder.HasOne(p => p.Employee)
                .WithMany(e => e.Payrolls)
                .HasForeignKey(p => p.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(p => new
            {
                p.EmployeeId,
                p.Year,
                p.Month
            })
            .IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Payroll_Month",
                    "[Month] BETWEEN 1 AND 12");

                t.HasCheckConstraint(
                    "CK_Payroll_Year",
                    "[Year] BETWEEN 1 AND 9999");
            });
        }
    }
}
