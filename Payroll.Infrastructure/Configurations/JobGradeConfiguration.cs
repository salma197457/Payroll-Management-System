using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class JobGradeConfiguration : IEntityTypeConfiguration<JobGrade>
    {
        public void Configure(EntityTypeBuilder<JobGrade> builder)
        {
            builder.ToTable("JobGrades");

            builder.HasKey(g => g.Id);

            builder.Property(g => g.Name)
                .IsRequired()
                .HasMaxLength(25);

            builder.Property(g => g.Salary)
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.RowVersion)
                 .IsRowVersion();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.HasIndex(g => g.Name)
                .IsUnique();

            builder.HasIndex(g => g.Salary)
                .IsUnique();
        }
    }
}
