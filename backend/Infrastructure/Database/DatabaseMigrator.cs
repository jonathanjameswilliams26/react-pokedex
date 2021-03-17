using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Backend.Infrastructure.Database
{
    public class DatabaseMigrator : IHostedService
    {
        private readonly ILogger<DatabaseMigrator> logger;
        private readonly IServiceScopeFactory serviceScopeFactory;

        public DatabaseMigrator(ILogger<DatabaseMigrator> logger, IServiceScopeFactory serviceScopeFactory)
        {
            this.logger = logger;
            this.serviceScopeFactory = serviceScopeFactory;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            logger.LogInformation("Running database migrator.");
            using (var scope = serviceScopeFactory.CreateScope())
            {
                try
                {
                    var db = scope.ServiceProvider.GetRequiredService<DB>();
                    logger.LogInformation("Running migrations.");
                    db.Database.Migrate();
                    logger.LogInformation("Finished runnning migrations.");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred creating the DB.");
                }
            }
            logger.LogInformation("Finished running database migrator.");
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
