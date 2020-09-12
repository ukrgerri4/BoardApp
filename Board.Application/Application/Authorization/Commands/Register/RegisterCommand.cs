using Board.Common.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Board.Application.Application.Authorization.Commands.Register
{
    public class RegisterCommand : IRequest<Result>
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
