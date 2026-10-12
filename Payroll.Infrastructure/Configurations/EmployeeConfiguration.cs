using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.ToTable("Employees");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(256);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11);

            builder.Property(e => e.Address)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.BirthDate)
                .IsRequired();

            builder.Property(e => e.JoinDate)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.HasQueryFilter(e => !e.IsDeleted);

            builder.HasOne(e => e.Department)
                .WithMany(d => d.Employees)
                .HasForeignKey(e => e.DepartmentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.JobGrade)
                .WithMany(g => g.Employees)
                .HasForeignKey(e => e.JobGradeId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
