using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using HelperTelegramBot.BotCommands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace HelperTelegramBot
{
    public class TgBotService : IHostedService
    {
        private ITelegramBotClient _botClient;
        private readonly ILogger _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;

        public TgBotService(ILogger<TgBotService> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;          
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            const string secretName = "tgbottoken";

            string keyVaultUri = _configuration["AzureKeyVault"];

            var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential(true));
            var secret = await client.GetSecretAsync(secretName);

            _botClient = new TelegramBotClient(secret.Value.Value);

            var receiverOptions = new ReceiverOptions
            {
                AllowedUpdates = new UpdateType[]{ UpdateType.Message, UpdateType.ChannelPost }
            };

            _botClient.StartReceiving(HandleUpdateAsync, HandleErrorAsync, receiverOptions, cancellationToken);

            _logger.LogInformation("Bot started");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _botClient.CloseAsync(cancellationToken);
            _logger.LogInformation("Bot stopped");
        }

        private async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string[] chatsWithAccess = _configuration.GetSection("ChatsWithAсcess").GetChildren().Select(item => item.Value).ToArray();
            
            if (!chatsWithAccess.Contains(update?.Message.Chat.Id.ToString()))
            {
                return;
            }

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                string message = update.Message.Text;

                int spaceIndex = message.IndexOf(' ');
                int newLineIndex = message.IndexOf('\n');
                int endCommandIndex = -1;

                if(spaceIndex != -1 || newLineIndex != -1)
                {
                    endCommandIndex = (spaceIndex == -1 || newLineIndex == -1) ? Math.Max(spaceIndex, newLineIndex) : Math.Min(spaceIndex, newLineIndex);
                }

                string commandWord = endCommandIndex == -1 ? message : message.Substring(0, endCommandIndex);

                var command = GetBotCommand(commandWord);

                if (command == null)
                {
                    await botClient.SendTextMessageAsync(update.Message.Chat, "Sorry, i don't support this command");
                    return;
                }

                await command.Execute(botClient, update);
            }
        }

        private async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {            
            _logger.LogError(exception?.Message);
        }

        private ICommand? GetBotCommand(string commandName)
        {           
            var command = _serviceProvider.GetServices<ICommand>().FirstOrDefault(c => c.CommandName == commandName);

            return command;
        }
    }
}
