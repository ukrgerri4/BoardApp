using Board.Game.Mafia.Game;
using Microsoft.Extensions.DependencyInjection;

namespace Board.Game.Mafia.Extension
{
    public static class MafiaGameExtensions
    {
        public static IServiceCollection AddMafiaGame(this IServiceCollection services)
        {
            services.AddScoped<IMafiaGameBuilder, MafiaGameBuilder>();
            services.AddTransient<MafiaGame>();

            return services;
        }
    }
}
