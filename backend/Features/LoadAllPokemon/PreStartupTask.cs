using Backend.Infrastructure;
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
        private readonly Database database;

        public PreStartupTask(ILogger<PreStartupTask> logger, PokeApi api/*, Database database*/)
        {
            this.logger = logger;
            this.api = api;
            //this.database = database;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //if (database.Pokemon.Any())
            //    return;

            logger.LogInformation("Loading all pokemon from PokeApi.");
            var pokemon = await api.GetAllPokemon();
            await database.AddRangeAsync(pokemon);
            logger.LogInformation("Finished loading all pokemon from PokeApi.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
