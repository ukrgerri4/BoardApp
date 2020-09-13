using BoardWebApp.Controllers.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BoardWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AccessController : ApiBaseController
    {
        public AccessController(IMediator mediator) : base(mediator) {}

        [HttpGet("page/{pageName}")]
        public async Task<IActionResult> HasPageAccess(string pageName)
        {
            return Ok(true);
        }

    }
}
