﻿using Cheetah.Domain.Aggregates.ProcessAggregate.Links;
using Cheetah.Domain.Entities.Facts;

namespace Cheetah.Infrastructure.Persistence.Data.Configurations.Process.Links;

public class L_ScenarioConditionConfiguration : IEntityTypeConfiguration<L_ScenarioCondition>
{
    public void Configure(EntityTypeBuilder<L_ScenarioCondition> builder)
    {
        builder.HasComment("Conditionals of each scenario");

        builder
            .HasOne(x => x.Scenario)
            .WithMany(x => x.ScenarioConditions)
            .HasForeignKey(x => x.FirstId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne<F_Condition>()
            .WithMany()
            .HasForeignKey(x => x.SecondId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
