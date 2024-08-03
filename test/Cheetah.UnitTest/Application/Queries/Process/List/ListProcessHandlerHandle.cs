﻿using Ardalis.SharedKernel;
using Cheetah.Application.Business.Queries.Process.List;
using Cheetah.Domain.Aggregates.ProcessAggregate.Specifications;
using Cheetah.Domain.Entities.Dimentions;
using NSubstitute;

namespace Cheetah.UnitTest.Application.Queries.Process.List;

public class ListProcessHandlerHandle
{
    IReadRepository<D_Process> _processRepository;
    List<D_Process> _processList;
    D_Process _process;
    ListProcessQuery _listProcessQuery;
    public ListProcessHandlerHandle()
    {
        _process = D_Process.SampleProcess;
        _processList = [_process];

        _processRepository = Substitute.For<IReadRepository<D_Process>>();

        _processRepository
            .ListAsync(Arg.Any<GetProcessSpec>(), CancellationToken.None)
            .Returns(_processList);

        _listProcessQuery = new ListProcessQuery(_processList.Select(x => x.Id));
    }

    [Fact]
    public async Task GetProcessList()
    {
        #region Arrange
        ListProcessHandler _listProcessHandler = new ListProcessHandler(processRepository: _processRepository);
        #endregion

        #region Act
        var _result = await _listProcessHandler
            .Handle(request: _listProcessQuery, CancellationToken.None);
        #endregion

        #region Assert
        Assert.Contains(_process, _result.Value);
        #endregion
    }
}
