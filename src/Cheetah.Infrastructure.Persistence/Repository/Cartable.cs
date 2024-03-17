﻿namespace Cheetah.Infrastructure.Persistence.Repository;

public class Cartable(ApplicationDbContext _db, ICopyClass _iCopyClass) : ICartable
{
    public async Task<List<F_Case>> GetCaseAsync(F_Case request)
    {
        var GeneralRequest = _db.F_Cases
            .Include(x => x.Creator)
            .Include(x => x.Requestor)
            .Include(x => x.Process)
            .Include(x => x.SelectedScenario)
            .Include(x => x.CaseConditions)
            .ThenInclude(x => x.Condition)
            .Include(x => x.CaseState)
            .Include(x => x.WorkItems)
            .ThenInclude(x => x.WorkItemConditions)
            .Include(x => x.WorkItems)
            .ThenInclude(x => x.WorkItemState)
            .Include(x => x.WorkItems)
            .ThenInclude(x => x.User)
            .AsNoTracking();

        if (request.Process is not null)
        {
            var _ProcessId = await _iCopyClass.GetSimpleClassId(_db.D_Processes, request.Process);
            GeneralRequest = GeneralRequest.Where(x => x.ProcessId == _ProcessId);
        }

        if (request.CaseState is not null)
        {
            var CaseStateId = await _iCopyClass.GetSimpleClassId(_db.D_CaseStates, request.CaseState);
            GeneralRequest = GeneralRequest.Where(x => x.CaseStateId == CaseStateId);
        }

        if (request.Id != 0)
        {
            GeneralRequest = GeneralRequest.Where(x => x.Id == request.Id);
        }

        if (request.ERPCode is not null)
        {
            GeneralRequest = GeneralRequest.Where(x => x.ERPCode == request.ERPCode);
        }

        var _casesList = await GeneralRequest.ToListAsync();

        return _casesList;
    }
    public async Task<List<CartableDTO>> GetCartable(CartableDTO cartableDTO, IQueryable<F_WorkItem> f_WorkItems)
    {

        if (cartableDTO.User is not null)
        {
            var UserID = await _iCopyClass.GetSimpleClassId(_db.D_Users, cartableDTO.User);

            f_WorkItems = f_WorkItems.Where(x => x.UserId == UserID);
        }
        if (cartableDTO.Process is not null)
        {
            var ProcessId = await _iCopyClass.GetSimpleClassId(_db.D_Processes, cartableDTO.Process);

            f_WorkItems = f_WorkItems.Where(x => x.Case.ProcessId == ProcessId);
        }
        if (cartableDTO.CaseState is not null)
        {
            var CaseStateId = await _iCopyClass.GetSimpleClassId(_db.D_CaseStates, cartableDTO.CaseState);

            f_WorkItems = f_WorkItems.Where(x => x.Case.CaseStateId == CaseStateId);
        }

        if (cartableDTO.Case is not null)
        {
            var CaseId = cartableDTO.Case.Id;
            f_WorkItems = f_WorkItems.Where(x => x.CaseId == CaseId);
        }

        if (cartableDTO.WorkItem is not null)
        {
            var workItemId = cartableDTO.WorkItem.Id;
            f_WorkItems = f_WorkItems.Where(x => x.Id == workItemId);
        }

        Int64 _PageSize = 0;

        Int64 _PageNumber = 0;

        if (cartableDTO.PageSize is not null)
        {
            _PageSize = cartableDTO.PageSize.Value;

            _PageNumber = cartableDTO.PageNumber.Value;

            f_WorkItems = f_WorkItems.Skip(Convert.ToInt32(_PageSize * (_PageNumber - 1))).Take(Convert.ToInt32(_PageSize));
        }

        var Records = await f_WorkItems.ToListAsync();

        var _TotalItems = Records.Count();

        var Inbox = Records
        .Select(x =>
        new CartableDTO()
        {
            PageSize = _PageSize,
            PageNumber = _PageNumber,
            TotalItems = _TotalItems,
            Process = _iCopyClass.GetSimpleClass(x.Case.Process),
            User = _iCopyClass.GetSimpleClass(x.User),
            Case = _iCopyClass.GetSimpleClass(x.Case),
            Requestor = _iCopyClass.GetSimpleClass(x.Case.Requestor),
            Creator = _iCopyClass.GetSimpleClass(x.Case.Creator),
            CaseState = _iCopyClass.GetSimpleClass(x.Case.CaseState),
            WorkItem = _iCopyClass.GetSimpleClass(x),
            WorkItemState = _iCopyClass.GetSimpleClass(x.WorkItemState),
            Task = _iCopyClass.GetSimpleClass(x.Task),
            ValidUserActions = x.Task.TaskFlows.SelectMany(a => a.Flow.FlowConditions, (a, b) => _iCopyClass.GetSimpleClass(b.Condition)),
            OccurredUserActions = x.WorkItemConditions.Select(x => _iCopyClass.GetSimpleClass(x.Condition)),
            Summary = string.Empty
        }
        );

        return Inbox.ToList();
    }
    public async Task<List<CartableDTO>> Inbox(CartableDTO cartableDTO)
    {
        try
        {
            var inboxQuery = _db.F_WorkItems
                      .Where(x =>
                      x.Case.EnableRecord &&
                      x.EnableRecord &&
                      x.WorkItemStateId == D_WorkItemState.Inbox.Id);

            var inbox = await GetCartable(cartableDTO, inboxQuery);

            return inbox;
        }
        catch (Exception ex)
        {

            return new List<CartableDTO>();
        }
    }
    public async Task<List<CartableDTO>> Outbox(CartableDTO cartableDTO)
    {
        try
        {
            var outBoxQuery = _db.F_WorkItems
                      .Where(x =>
                      x.Case.EnableRecord == true &&
                      x.EnableRecord == true &&
                      x.WorkItemStateId == D_WorkItemState.Sent.Id);

            var outBox = await GetCartable(cartableDTO, outBoxQuery);

            return outBox;
        }
        catch (Exception ex)
        {

            return new List<CartableDTO>();
        }
    }
}