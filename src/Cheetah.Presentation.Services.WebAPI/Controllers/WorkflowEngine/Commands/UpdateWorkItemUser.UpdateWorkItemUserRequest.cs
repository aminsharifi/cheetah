﻿using Cheetah.Application.Business.DTOs.Case;

namespace Cheetah.Presentation.Services.WebAPI.Controllers.WorkflowEngine.Commands;

public class UpdateWorkItemUserRequest : UpdateWorkItemUser_Request
{
    public const string Route = "/WorkflowEngine/Commands/" + nameof(UpdateWorkItemUser);
}
