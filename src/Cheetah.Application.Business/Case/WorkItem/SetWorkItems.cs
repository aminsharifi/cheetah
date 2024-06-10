﻿namespace Cheetah.Application.Business.Case.WorkItem;

public static class SetWorkItems
{
    public static async Task<Result<F_Case>> Handle
        (ISender iSender, IRepository<F_Task> taskRepository, 
        F_Case Current_Case, F_WorkItem Current_WorkItem)
    {
        Current_Case = await SelectScenario.Handle(iSender, Current_Case);

        Current_Case = await CreateWorkItems.Handle(iSender, Current_Case);

        F_WorkItem _workItem = new();

        _workItem.SetCase(Current_Case);

        await SetCurrentAssignment.Handle(iSender: iSender, taskRepository: taskRepository, 
            Current_WorkItem: Current_WorkItem);

        return Current_Case;
    }
}
