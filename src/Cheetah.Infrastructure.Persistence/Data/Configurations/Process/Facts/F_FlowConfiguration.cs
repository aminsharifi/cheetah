﻿using Cheetah.Domain.Entities.Facts;

namespace Cheetah.Infrastructure.Persistence.Data.Configurations.Process.Facts;

public class F_FlowConfiguration : IEntityTypeConfiguration<F_Flow>
{
    public void Configure(EntityTypeBuilder<F_Flow> builder)
    {
        builder.HasComment("Flow of BPMN for transferring between tasks");

        //builder
        //    .Property(e => e.CaseStateId)
        //    .HasColumnOrder(101);
    }
}
