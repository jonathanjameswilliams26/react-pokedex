using Backend.Features.LoadAllPokemon.DependencyInjection;
using Backend.Infrastructure.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((ctx, services) =>
                {
                    // Need to add this here so it runs before the web host
                    // starts listening to HTTP requests.
                    // See: https://andrewlock.net/running-async-tasks-on-app-startup-in-asp-net-core-3/
                    AddStartupTasks(services, ctx.Configuration);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

        static void AddStartupTasks(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DB>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Database")));
            services.AddHostedService<DatabaseMigrator>();
            services.AddLoadAllPokemonFeature();
        }
    }
}
