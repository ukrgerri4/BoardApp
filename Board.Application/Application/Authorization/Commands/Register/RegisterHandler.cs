using Board.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Board.Application.Application.Authorization.Commands.Register
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly UserManager<IdentityUser> _userManager;

        public RegisterHandler(UserManager<IdentityUser> userManager)
        {
            this._userManager = userManager;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = new IdentityUser
            {
                UserName = request.UserName,
                Email = request.Email
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                return Result.Fail(result.ToString());
            }
            
            return Result.Ok();
        }
    }
}
