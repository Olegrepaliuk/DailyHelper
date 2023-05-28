using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HelperTelegramBot.BotCommands
{
    public interface ICommand
    {
        Task Execute(ITelegramBotClient botClient, Update update);

        string CommandName { get; }
    }
}
