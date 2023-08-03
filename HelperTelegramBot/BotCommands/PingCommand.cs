using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HelperTelegramBot.BotCommands
{
    public class PingCommand : ICommand
    {
        public string CommandName => "/ping";

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            string answerText = $"How can I help you?";

            await botClient.SendTextMessageAsync(update.Message.Chat, answerText);
        }
    }
}
