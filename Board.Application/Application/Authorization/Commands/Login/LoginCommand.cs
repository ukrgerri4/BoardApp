using Board.Common.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.Application.Application.Authorization.Commands.Login
{
    public class LoginCommand: IRequest<Result<LoginResponse>>
    {
        [Required]
        public string UserName { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
