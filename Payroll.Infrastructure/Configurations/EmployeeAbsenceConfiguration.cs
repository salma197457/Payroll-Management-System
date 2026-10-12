using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class EmployeeAbsenceConfiguration : IEntityTypeConfiguration<EmployeeAbsence>
    {
        public void Configure(
       EntityTypeBuilder<EmployeeAbsence> builder)
        {
            builder.ToTable("EmployeeAbsences");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.Date)
                .IsRequired();

            builder.HasOne(a => a.Employee)
                .WithMany(e => e.Absences)
                .HasForeignKey(a => a.EmployeeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => new
            {
                a.EmployeeId,
                a.Date
            })
            .IsUnique();
        }
    }
}
