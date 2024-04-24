﻿namespace Cheetah.Presentation.Services.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class RequestController : ControllerBase
{
    public ILogger<RequestController> _logger;
    public ICartable _iCartable;
    public IWorkItem _iWorkItem;
    public ICopyClass _iCopyClass;
    public ISync _iSync;
    public IMediator _mediator;
    public IReadRepository<F_WorkItem> _workItemRepository;
    public RequestController(ILogger<RequestController> GLogger,
        ICartable GICartable, IWorkItem GIWorkItem,
        ICopyClass GICopyClass, ISync GISync, IMediator GMediator,
        IReadRepository<F_WorkItem> WorkItemRepository
        )
    {
        _logger = GLogger;
        _iCartable = GICartable;
        _iWorkItem = GIWorkItem;
        _iCopyClass = GICopyClass;
        _iSync = GISync;
        _mediator = GMediator;
        _workItemRepository = WorkItemRepository;
    }

    [HttpGet(nameof(Health))]
    public String Health()
    {
        return "Ok!";
    }

    [HttpPost(nameof(CreateRequest))]
    public async Task<CreateRequest_Output> CreateRequest([FromBody] CreateRequest_Input request)
    {
        _logger.LogInformation("started " + nameof(CreateRequest) + " {@" + nameof(CreateRequest) + "}", request);

        #region Input

        SimpleClassDTO _case = request.Case.GetSimpleClass<SimpleClassDTO>();
        SimpleClassDTO _creator = request.Creator.GetSimpleClass<SimpleClassDTO>();
        SimpleClassDTO _requestor = request.Requestor.GetSimpleClass<SimpleClassDTO>();
        SimpleClassDTO _process = request.Process.GetSimpleClass<SimpleClassDTO>();
        List<GRPC_Condition> _caseConditions = request.Conditions;
        List<GRPC_Condition> _workItemConditions = request.WorkItem.OccurredUserActions;
        SimpleClassDTO _workItemUser = request.WorkItem.User.GetSimpleClass<SimpleClassDTO>();

        #endregion

        var Outputresult = await _iWorkItem.CreateRequestAsync(_case, _creator, _requestor, _process, _caseConditions, _workItemUser, _workItemConditions);

        #region Output

        var OutputState = Outputresult.SimpleClassDTO;

        CreateRequest_Output output_Request = new();

        output_Request.OutputState = OutputState.GetBaseClassWithName();

        if (!Outputresult.Result.IsSuccess)
        {
            return output_Request;
        }

        long _createdCaseId = Outputresult.Result.Value;

        GetCase_Input _getCase_Input = new()
        {
            Case = new GRPC_BaseClass()
            {
                Id = _createdCaseId
            }
        };

        GetCase_Output _getCase_Output = await GetCase(_getCase_Input);

        output_Request.Case = _getCase_Output.Case;

        #endregion

        _logger.LogInformation("Ended " + nameof(CreateRequest) + " {@" + nameof(CreateRequest) + "}", output_Request);

        return output_Request;
    }

    [HttpPost(nameof(GetCase))]
    public async Task<GetCase_Output> GetCase([FromBody] GetCase_Input request)
    {
        _logger.LogInformation("started " + nameof(GetCase) + " {@" + nameof(GetCase) + "}", request);

        #region Input
        SimpleClassDTO _request = request.Case?.GetSimpleClass<SimpleClassDTO>();
        //_request.CaseState = request.CaseState?.GetSimpleClass<D_CaseState>();
        SimpleClassDTO _process = request.Process?.GetSimpleClass<SimpleClassDTO>();
        #endregion

        var _requests = await _iCartable.GetCaseAsync(_request, _process);

        #region Output

        GetCase_Output output_Request = new();

        if (!_requests.Value.Any())
        {
            output_Request.OutputState = OutputState<Boolean>
                .NotFoundErrorCreateRequest(false)
                .SimpleClassDTO
                .GetBaseClassWithName();

            return output_Request;
        }

        var _selectedRequests = _requests.Value.FirstOrDefault();

        output_Request.Case = new();

        output_Request.Case.Base = _selectedRequests.GetBaseClassWithDate();
        output_Request.Case.CaseState = _selectedRequests.CaseState.GetBaseClassWithName();
        output_Request.Case.RequestorId = _selectedRequests.RequestorId;
        output_Request.Case.CreatorId = _selectedRequests.CreatorId;
        output_Request.Case.ProcessId = _selectedRequests.ProcessId;

        foreach (var WorkItem in _selectedRequests.WorkItems)
        {
            GRPC_Task _gRPC_Task = new();
            GRPC_WorkItem _gRPC_WorkItem = new();
            _gRPC_Task.WorkItems = new();
            output_Request.Case.Tasks = new();
            _gRPC_WorkItem.Base = WorkItem.GetBaseClassWithDate();
            _gRPC_WorkItem.WorkItemState = WorkItem.WorkItemState.GetBaseClassWithName();
            _gRPC_WorkItem.User = new GRPC_BaseClassWithName() { Id = WorkItem.UserId };
            _gRPC_WorkItem.OccurredUserActions = new();
            foreach (var WorkItemCondition in WorkItem.WorkItemConditions)
            {
                GRPC_Condition _occurredUserAction = new() { Base = new() { Id = WorkItemCondition.SecondId } };
                _gRPC_WorkItem.OccurredUserActions.Add(_occurredUserAction);
            }
            _gRPC_Task.WorkItems.Add(_gRPC_WorkItem);
            output_Request.Case.Tasks.Add(_gRPC_Task);
        }

        output_Request.OutputState = OutputState<Boolean>
            .Success(nameof(GetCase), true)
            .SimpleClassDTO.GetBaseClassWithName();

        #endregion

        _logger.LogInformation("Ended " + nameof(GetCase) + " {@" + nameof(GetCase) + "}", output_Request);

        return output_Request;
    }
    [HttpPost(nameof(Inbox))]
    public async Task<Cartable_Output> Inbox([FromBody] Cartable_Input request)
    {
        return await Inbox(request);
    }
    [HttpPost(nameof(Outbox))]
    public async Task<Cartable_Output> Outbox([FromBody] Cartable_Input request)
    {
        return await Outbox(request);
    }
    [HttpPost(nameof(PerformRequest))]
    public async Task<PerformRequest_Output> PerformRequest([FromBody] PerformRequest_Input request)
    {
        return await PerformRequest(request);
    }
    [HttpPost(nameof(GetAllByName))]
    public async Task<GetAllByName_Output> GetAllByName([FromBody] GetAllByName_Input request)
    {
        return await GetAllByName(request);
    }
    [HttpPost(nameof(SetCaseTaskUser))]
    public async Task<SetCaseTaskUser_Output> SetCaseTaskUser([FromBody] SetCaseTaskUser_Input request)
    {
        return await SetCaseTaskUser(request);
    }
    [HttpPost(nameof(SyncEntity))]
    public async Task<SyncEntity_Output> SyncEntity([FromBody] SyncEntity_Input request)
    {
        return await SyncEntity(request);
    }
    [HttpPost(nameof(SyncLink))]
    public async Task<SyncLink_Output> SyncLink([FromBody] SyncLink_Input request)
    {
        return await SyncLink(request);
    }
    [HttpPost(nameof(SyncCondition))]
    public async Task<SyncCondition_Output> SyncCondition([FromBody] SyncCondition_Input request)
    {
        return await SyncCondition(request);
    }
    #region Private methods
    private async Task<GRPC_Case> GetCase(F_Case _case, D_Process? Process, D_User? Creator, D_User? Requestor)
    {
        GRPC_Case _gRPC_Case = new()
        {
            Base = _case?.GetBaseClassWithDate(),
            CaseState = _case?.CaseState?.GetBaseClassWithName(),
            //Process = Process?.GetBaseClassWithName(),
            //Creator = Creator.GetBaseClassWithName(),
            //Requestor = Requestor.GetBaseClassWithName()
        };

        #region Tasks

        var _getTasks = await _mediator.Send(new GetTasksFromScenarioQuery(_case.SelectedScenarioId));

        var Tasks = _getTasks.Value.ToList();

        _gRPC_Case.Tasks.AddRange(
            Tasks.Select(x => new GRPC_Task()
            {
                Base = x.GetBaseClassWithName()
            })
            );

        #endregion

        #region L_WorkItem

        foreach (var Task in _gRPC_Case.Tasks)
        {
            Task.WorkItems.AddRange(
            _case?.WorkItems
                .Where(x => x.TaskId == Task.Base.Id)
                .Select(x => new GRPC_WorkItem()
                {
                    Base = x.GetBaseClassWithDate(),
                    WorkItemState = x.WorkItemState?.GetBaseClassWithName()
                })
                );

            F_Task _task = Task.Base.GetSimpleClass<F_Task>();

            foreach (var workItem in Task.WorkItems)
            {
                var _WorkItem = _case?.WorkItems
                    .Where(x => x.Id == workItem.Base.Id)
                    .SingleOrDefault();

                var _conditions = GetConditions(_WorkItem, _task);                
                workItem.OccurredUserActions.AddRange(_conditions.OccurredUserActions);
            }
        }

        #endregion

        return _gRPC_Case;
    }
    private GRPC_WorkItem GetConditions(F_WorkItem workItem, F_Task Task)
    {
        GRPC_WorkItem gRPC_WorkItem = new();

        /*
                #region OccurredUserActions


                var _occurredUserActions = workItem
                    .WorkItemConditions
                    .Select(x => x.Condition)
                .GetConditions();


                gRPC_WorkItem.OccurredUserActions.AddRange(_occurredUserActions);


                #endregion

                #region ValidUserActions
                var _validUserActions = Task.TaskFlows
                    .SelectMany(x => x.Flow.FlowConditions, (x, y) => y.Condition)
                .GetConditions();

                gRPC_WorkItem.ValidUserActions.AddRange(_validUserActions);
                #endregion
         */

        return gRPC_WorkItem;
    }
    private async Task<Cartable_Output> Cartable(Cartable_Input request, CartableProperty cartableProperty)
    {
        _logger.LogInformation("started " + nameof(Cartable));
        _logger.LogInformation("{@InputCartable}", request);

        #region Input

        var _assignee = request.Assignee?.GetSimpleClass<SimpleClassDTO>();
        var _process = request.Process?.GetSimpleClass<SimpleClassDTO>();
        var _caseState = request.CaseState?.GetSimpleClass<SimpleClassDTO>();
        var _case = request.Case?.GetSimpleClass<SimpleClassDTO>();
        var _workItem = request.WorkItem?.GetSimpleClass<SimpleClassDTO>();

        var cartableDTO = new CartableDTO()
        {
            User = _assignee,
            Process = _process,
            Case = _case,
            WorkItem = _workItem,
            CaseState = _caseState,
            PageSize = request.PageSize,
            PageNumber = request.PageNumber,
        };

        #endregion

        var OutputRequest = ((cartableProperty == CartableProperty.Inbox) ?
           await _iCartable.InboxAsync(cartableDTO) :
           await _iCartable.OutboxAsync(cartableDTO));

        #region Output
        Cartable_Output _OutputCartable = new();


        if (OutputRequest.Value.Any())
        {
            _OutputCartable.TotalItems = OutputRequest.Value.FirstOrDefault()?.TotalItems.Value;
            _OutputCartable.PageSize = OutputRequest.Value.FirstOrDefault()?.PageSize.Value;
            _OutputCartable.PageNumber = OutputRequest.Value.FirstOrDefault()?.PageNumber.Value;


            foreach (var outputRequestItem in OutputRequest.Value)
            {
                GRPC_Case _Case = new()
                {
                    Base = outputRequestItem.Case.GetBaseClassWithDate(),
                    CaseState = outputRequestItem.CaseState.GetBaseClassWithName(),
                    //Creator = outputRequestItem.Creator.GetBaseClassWithName(),
                    //Requestor = outputRequestItem.Requestor.GetBaseClassWithName(),
                    //Process = outputRequestItem.Process.GetBaseClassWithName()
                };

                GRPC_Task _task = new();

                _task.Base = outputRequestItem.Task.GetBaseClassWithName();

                GRPC_WorkItem _gRPC_WorkItem = new();

                _gRPC_WorkItem.Base = outputRequestItem.WorkItem.GetBaseClassWithDate();

                _gRPC_WorkItem.User = outputRequestItem.User.GetBaseClassWithName();

                _gRPC_WorkItem.WorkItemState = outputRequestItem.WorkItemState.GetBaseClassWithName();

                var _workItemId = _gRPC_WorkItem.Base.Id;

                GetEntitySpec<F_WorkItem> _getEntitySpec = new(_gRPC_WorkItem.Base.GetSimpleClass<F_WorkItem>());

                var _retriveworkItem = await _workItemRepository.FirstOrDefaultAsync(_getEntitySpec);

                var _f_task = _task.Base.GetSimpleClass<F_Task>();

                var _conditions = GetConditions(_retriveworkItem, _f_task);                
                _gRPC_WorkItem.OccurredUserActions.AddRange(_conditions.OccurredUserActions);


                _task.WorkItems.Add(_gRPC_WorkItem);

                _Case.Tasks.Add(_task);

                _OutputCartable.Cases.Add(_Case);
            }
        }
        #endregion

        _logger.LogInformation("Ended " + nameof(Cartable));
        _logger.LogInformation("{@OutputCartable}", _OutputCartable);

        _OutputCartable.OutputState = OutputState<Boolean>
            .Success(nameof(Cartable), true)
            .SimpleClassDTO.GetBaseClassWithName();

        return _OutputCartable;
    }
    #endregion
}