using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Payroll.Domain.Rules;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payroll.Infrastructure.Configurations
{
    public class ExperienceIncentiveConfiguration : IEntityTypeConfiguration<ExperienceIncentive>
    {
        public void Configure(
      EntityTypeBuilder<ExperienceIncentive> builder)
        {
            builder.ToTable("ExperienceIncentives");

            builder.HasKey(i => i.Id);

            builder.Property(i => i.MinimumYears)
                .IsRequired();

            builder.Property(i => i.Percentage)
                .HasPrecision(5, 2)
                .IsRequired();

            builder.HasIndex(i => i.MinimumYears)
                .IsUnique();

            builder.Property(e => e.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()")
                .ValueGeneratedOnAdd();

            builder.Property(e => e.RowVersion)
                .IsRowVersion();

            builder.Property(e => e.UpdatedAt)
                .IsRequired(false);

            builder.ToTable(t =>
            {
                t.HasCheckConstraint(
                    "CK_Experience_MinYears",
                    "[MinimumYears] >= 0");

                t.HasCheckConstraint(
                    "CK_Experience_Percentage",
                    "[Percentage] >= 0 AND [Percentage] <= 100");
            });
        }
    }
}
