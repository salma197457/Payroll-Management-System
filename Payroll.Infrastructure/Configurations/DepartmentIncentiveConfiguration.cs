using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class DepartmentIncentiveConfiguration : IEntityTypeConfiguration<DepartmentIncentive>
    {
        public void Configure(
       EntityTypeBuilder<DepartmentIncentive> builder)
        {
            builder.ToTable("DepartmentIncentives");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Percentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.HasIndex(i => i.DepartmentId)
                .IsUnique();

            builder.ToTable(t =>
                t.HasCheckConstraint(
                    "CK_DepartmentIncentive_Percentage",
                    "[Percentage] >= 0 AND [Percentage] <= 100"));
        }
    }
}
