using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class AbsenceDeducationRuleConfiguration : IEntityTypeConfiguration<AbsenceDeductionRule>
    {
        public void Configure(EntityTypeBuilder<AbsenceDeductionRule> builder)
        {
            builder.ToTable("AbsenceDeducationRules");

            builder.HasKey(r => r.Id);

            builder.Property(r => r.MinimumDays)
                .IsRequired();

            builder.Property(r => r.Percentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.Property(r => r.Type)
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.HasIndex(r => r.MinimumDays)
                .IsUnique();

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_AbsenceRule_MinDays",
                    "[MinimumDays] >= 0");

                t.HasCheckConstraint(
                    "CK_AbsenceRule_Percentage",
                    "[Percentage] >= 0 AND [Percentage] <= 100");

                t.HasCheckConstraint(
                    "CK_AbsenceRule_Type",
                    "[Type] IN ('Incentive', 'Deduction')");
            });
        }
    }
}
