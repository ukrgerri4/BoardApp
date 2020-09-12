using Microsoft.AspNetCore.Identity;

namespace Board.Application.Interfaces.Services
{
    public interface ITokenService
    {
        string GenerateToken(IdentityUser user);
    }
}
