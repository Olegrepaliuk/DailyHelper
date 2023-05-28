using HelperTelegramBot.Http;
using HelperTelegramBot.Repositories;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace HelperTelegramBot.BotCommands
{
    public class DayCommand : ICommand
    {
        private readonly ITimeDataClient _timeDataClient;
        private readonly IUserRepository _userRepository;
        private readonly ILogger _logger;

        public string CommandName => "/dayinfo";

        public DayCommand(ITimeDataClient timeDataClient, IUserRepository userRepository, ILogger<DayCommand> logger)
        {
            _timeDataClient = timeDataClient;
            _userRepository = userRepository;
            _logger = logger;
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {

            string answerText = "";

            try
            {
                var user = _userRepository.GetUserByChatId(Convert.ToInt32(update.Message.Chat.Id));

                if (user == null)
                {
                    throw new ArgumentNullException("Unable to find user");
                }

                var currentDate = update.Message.Date;

                if (user.TimeZone != null)
                {
                    TimeZoneInfo timeZone = TimeZoneInfo.FindSystemTimeZoneById(user.TimeZone);
                    currentDate = TimeZoneInfo.ConvertTimeFromUtc(currentDate, timeZone);
                }

                var dayInfoResult = await _timeDataClient.GetDayInfo(currentDate);

                answerText = String.Join(
                                    Environment.NewLine,
                                    $"Day: {dayInfoResult.WeekDayName}, {dayInfoResult.Date.ToString("yyyy-MM-dd")}",
                                    $"Day number: {dayInfoResult.DayNumberInYear}",
                                    $"Week number: {dayInfoResult.WeekNumber}",
                                    $"Days to New Year: {dayInfoResult.DaysToNewYear}",
                                    $"Year completed: {dayInfoResult.YearCompletedPercent}%");
            }
            catch (Exception ex)
            {
                answerText = "An error occured while completing your request";
                _logger.LogError(ex.Message);
            }

            
            await botClient.SendTextMessageAsync(update.Message.Chat, answerText);
        }


    }
}
