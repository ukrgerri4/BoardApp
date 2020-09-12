using Board.Application.Interfaces.Services;
using Board.Common.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using System.Threading.Tasks;

namespace Board.Application.Application.Authorization.Commands.Login
{
    public class LoginHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public LoginHandler(
            ITokenService tokenService,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager)
        {
            this._tokenService = tokenService;
            this._userManager = userManager;
            this._signInManager = signInManager;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            { 
                return Result.Fail<LoginResponse>("User name not found.");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(request.UserName, request.Password, false, true);

            if (!signInResult.Succeeded)
            {
                return Result.Fail<LoginResponse>("Sign in failed.");
            }

            var accessToken = _tokenService.GenerateToken(user);

            return Result.Ok(new LoginResponse { AccessToken = accessToken });
        }
    }
}
