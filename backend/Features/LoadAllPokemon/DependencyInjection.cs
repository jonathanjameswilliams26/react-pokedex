using Microsoft.Extensions.DependencyInjection;

namespace Backend.Features.LoadAllPokemon.DependencyInjection
{
    public static class DependencyInjection
    {
        public static void AddLoadAllPokemonFeature(this IServiceCollection services)
        {
            services.AddHostedService<PreStartupTask>();
            services.AddHttpClient<PokeApi>();
        }
    }
}
