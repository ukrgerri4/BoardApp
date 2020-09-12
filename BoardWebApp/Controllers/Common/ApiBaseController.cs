using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BoardWebApp.Controllers.Common
{
    public class ApiBaseController : ControllerBase
    {
        protected IMediator Mediator { get; }

        protected ApiBaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}
