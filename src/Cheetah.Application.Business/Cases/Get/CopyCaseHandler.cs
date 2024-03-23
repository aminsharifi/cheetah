﻿namespace Cheetah.Application.Business.Cases.Get;

public class CopyCaseHandler(
    IReadRepository<D_User> _userRepository,
    IReadRepository<D_Process> _processRepository,
    IMediator _mediator) : IQueryHandler<CopyCaseQuery, Result<F_Case>>
{
    public async Task<Result<F_Case>> Handle(CopyCaseQuery request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request.input.Creator);
        Guard.Against.Null(request.input.Requestor);
        Guard.Against.Null(request.input.Process);

        F_Case _case = new();

        _case.ERPCode = request.input?.ERPCode;

        if (request.input.CreatorId is not null or 0)
        {
            _case.CreatorId = request.input.CreatorId;
        }
        else
        {
            var _userSpec = new GetEntitySpec<D_User>(request.input.Creator);
            _case.CreatorId = (await _userRepository.FirstOrDefaultAsync(_userSpec, cancellationToken)).Id;
        }

        if (request.input.RequestorId is not null or 0)
        {
            _case.RequestorId = request.input.RequestorId;
        }
        else
        {
            var _userSpec = new GetEntitySpec<D_User>(request.input.Requestor);
            _case.RequestorId = (await _userRepository.FirstOrDefaultAsync(_userSpec, cancellationToken)).Id;
        }

        if (request.input.ProcessId is not null or 0)
        {
            _case.ProcessId = request.input.ProcessId;
        }
        else
        {
            var _processSpec = new GetEntitySpec<D_Process>(request.input.Process);
            _case.ProcessId = (await _processRepository.FirstOrDefaultAsync(_processSpec, cancellationToken)).Id;
        }

        var _conditions = request.input.CaseConditions.Select(x => x.Condition);

        foreach (var _condition in _conditions)
        {
            var _getCondition = await _mediator.Send(new CopyConditionQuery(_condition));

            _case.CaseConditions.Add(new()
            {
                SecondId = _getCondition.Value.Id
            });
        }

        return _case;
    }
}
