using Backend.Infrastructure;
using Backend.Infrastructure.Database;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Features.LoadAllPokemon
{
    public class PreStartupTask : IHostedService
    {
        private readonly ILogger<PreStartupTask> logger;
        private readonly PokeApi api;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public PreStartupTask(ILogger<PreStartupTask> logger, PokeApi api, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.api = api;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = serviceScopeFactory.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<DB>();
                if (db.Pokemon.Any())
                    return;

                logger.LogInformation("Loading all pokemon from PokeApi.");
                var pokemon = await api.GetAllPokemon();
                await db.Pokemon.AddRangeAsync(pokemon);
                await db.SaveChangesAsync();
                logger.LogInformation("Finished loading all pokemon from PokeApi.");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
