using HelperTelegramBot;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration.Json;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using HelperTelegramBot.Http;
using System.Reflection;
using HelperTelegramBot.BotCommands;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using HelperTelegramBot.Repositories;

class Program
{
    static void Main(string[] args)
    {

        Host.CreateDefaultBuilder(args)
            .ConfigureLogging((hostContext, logging) =>
            {
                logging.ClearProviders();
                logging.AddConsole();
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddLogging();

                services.AddApplicationInsightsTelemetryWorkerService();

                services.AddHttpClient<ITimeDataClient, HttpTimeDataClient>();

                var botCommandTypes = Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => typeof(ICommand).IsAssignableFrom(t) && !t.IsInterface);

                foreach (var type in botCommandTypes)
                {
                    services.AddTransient(typeof(ICommand), type);
                }

                var dbConnString = hostContext.Configuration.GetConnectionString("DbConnection");

                services.AddDbContext<AppDbContext>(options =>
                    options.UseCosmos(dbConnString, "Default1"));

                services.AddScoped<IUserRepository, UserRepository>();

                services.AddHostedService<TgBotService>();
            })
            .Build()
            .Run();           
    }
}
