using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Entities;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Departments");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Name)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(d => d.Name)
                .IsUnique()
                .HasFilter("[IsDeleted] = 0");

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Property(e => e.IsDeleted)
                .HasDefaultValue(false);

            builder.HasOne(d => d.Incentive)
                .WithOne(i => i.Department)
                .HasForeignKey<DepartmentIncentive>(
                    i => i.DepartmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(e => !e.IsDeleted);

        }
    }
}
