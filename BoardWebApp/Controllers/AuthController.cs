using Board.Application.Application.Authorization.Commands.Login;
using Board.Application.Application.Authorization.Commands.Register;
using BoardWebApp.Controllers.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BoardApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ApiBaseController
    {
        public AuthController(IMediator mediator) : base(mediator) {}

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand model)
        {
            var result = await Mediator.Send(model);

            if (result.IsFail) { return BadRequest(result.Error); }

            return StatusCode(201);
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginCommand model)
        {
            var result = await Mediator.Send(model);

            if (result.IsFail) { return Unauthorized(); }

            return Ok(result.Value);
        }
    }
}
