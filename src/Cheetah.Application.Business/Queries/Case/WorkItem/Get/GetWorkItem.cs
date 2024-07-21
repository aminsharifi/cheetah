﻿using Cheetah.Application.Business.Queries.Condition.Condition.Get;
using Cheetah.Domain.Aggregates.CaseAggregate.Links;
using Cheetah.Domain.Aggregates.ConditionAggregate.DTOs;
using Cheetah.Domain.Common.DTOs;
using Cheetah.Domain.Common.Specifications;
using Cheetah.Domain.Entities.Dimentions;
using Cheetah.Domain.Entities.Facts;

namespace Cheetah.Application.Business.Queries.Case.WorkItem.Get;

public static class GetWorkItem
{
    public static async Task<F_WorkItem> Apply(ISender iSender,
        SimpleClassDTO WorkItemUser,
        List<ConditionDTO> workItemConditions,
        SimpleClassDTO WorkItemBase,
        IReadRepository<D_User> _userRepository, F_WorkItem workItem,
        CancellationToken cancellationToken)
    {
        var _workItemUserSpec = new GetIdEntitySpec<D_User>(WorkItemUser);
        var _workItemUserId = await _userRepository.FirstOrDefaultAsync(_workItemUserSpec, cancellationToken);

        workItem.SetUserId(_workItemUserId);

        workItem
            .SetName(WorkItemBase.Name)
            .SetDisplayName(WorkItemBase.DisplayName);

        workItem.WorkItemConditions?.Clear();

        await Parallel.ForEachAsync(workItemConditions, async (_condition, _cancellatoin) =>
        {
            var _getCondition = await iSender.Send(new GetConditionIdQuery(_condition));

            workItem.WorkItemConditions?.Add((L_WorkItemCondition)new L_WorkItemCondition().SetSecondId(_getCondition.Value));
        });

        return workItem;
    }
}
