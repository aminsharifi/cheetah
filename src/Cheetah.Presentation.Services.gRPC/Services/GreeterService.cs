﻿using Cheetah.Application.Business.Interfaces;
using Cheetah_GrpcService;

namespace Cheetah.Presentation.Services.gRPC.Services;

public class GreeterService(ILogger<GreeterService> _logger,
        ITableCRUD _isimpleClassRepository,
        ICartable _iCartable, ICopyClass _iCopyClass) : Greeter.GreeterBase
{
    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        var helloReply = new HelloReply()
        {
            Message = "Hello " + request.Name
        };

        return Task.FromResult(helloReply);
    }
}