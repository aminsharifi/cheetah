﻿namespace Cheetah.Application.Business.Case.WorkItem.Specifications;

public class GetWorkItemSpec : GetEntitySpec<F_WorkItem>
{
    public GetWorkItemSpec(SimpleClassDTO input, Boolean EnableTrack) : base(input, EnableTrack)
    {
        Query
            .Include(x => x.Case)
            .ThenInclude(x => x.WorkItems);

        Query
            .Include(x => x.WorkItemConditions);
    }
}