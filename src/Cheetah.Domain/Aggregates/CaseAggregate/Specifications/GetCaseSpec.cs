﻿namespace Cheetah.Application.Business.Case.Specifications;
public class GetCaseSpec : Specification<F_Case>
{
    public GetCaseSpec(long? processId, long? eRPCode)
    {
        Query
            .Where(x => x.ProcessId == processId)
            .Where(x => x.ERPCode == eRPCode)
            .Where(x => x.CaseStateId == D_CaseState.Ongoing.Id || x.CaseStateId == D_CaseState.Editing.Id)
            .Where(x => x.EnableRecord == true);

        Query
            .AsNoTracking();
    }
}