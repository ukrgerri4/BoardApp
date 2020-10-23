using Microsoft.Extensions.DependencyInjection;

namespace Board.Game.Resistance.Extension
{
    public static class ResistanceGameExtensions
    {
        public static IServiceCollection AddMafiaGame(this IServiceCollection services)
        {
            return services;
        }
    }
}
