using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HelperTelegramBot.BotCommands
{
    public class StartCommand : ICommand
    {
        public string CommandName => "/start";

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            string username = update.Message.Chat.Username;
            string answerText = $"Hi, {username}. I am here to help with some small routines or just have fun:)";

            await botClient.SendTextMessageAsync(update.Message.Chat, answerText);
        }
    }
}
