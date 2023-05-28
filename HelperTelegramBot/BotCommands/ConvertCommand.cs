using HelperTelegramBot.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenAI_API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TimeService.Models;

namespace HelperTelegramBot.BotCommands
{
    public class ConvertCommand : ICommand
    {
        public string CommandName => "/convert";

        private readonly IConfiguration _configuration;
        private readonly ITimeDataClient _timeDataClient;
        private readonly ILogger _logger;

        public ConvertCommand(IConfiguration configuration, ITimeDataClient timeDataClient, ILogger<ConvertCommand> logger)
        {
            _configuration = configuration;
            _timeDataClient = timeDataClient;
            _logger = logger;
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {

            string answerText = "";

            try
            {
                string userInput = update.Message.Text.Substring(CommandName.Length).Trim();

                string[] commandWords = userInput.Split(' ');
              
                const int timeUnitFromValueIndex = 0;
                const int timeUnitFromIndex = 1; // schema: 1 HOUR = SECONDS
                const int timeUnitToIndex = 3;

                double unitFromValue = Convert.ToDouble(commandWords[timeUnitFromValueIndex]);

                string timeUnitFrom = ParseUnit(commandWords[timeUnitFromIndex]);

                string timeUnitTo = ParseUnit(commandWords[timeUnitToIndex]);

                var result = await _timeDataClient.ConvertTime(timeUnitFrom, timeUnitTo, unitFromValue);

                answerText = $"{result.UnitFromValue} {result.UnitFrom}(S) = {result.Result} {result.UnitTo}(S)";

            }
            catch (ArgumentException ex)
            {
                answerText = "Unable to convert value. Please, check input data";
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                answerText = "An error occured while completing your request";
                _logger.LogError(ex.Message);
            }


            await botClient.SendTextMessageAsync(update.Message.Chat, answerText);
        }

        private string ParseUnit(string timeUnit)
        {
            TimeUnit timeUnitEnum;

            try
            {
                timeUnitEnum = (TimeUnit)Enum.Parse(typeof(TimeUnit), timeUnit.ToUpper());
            }
            catch (Exception e)
            {
                if (!string.IsNullOrEmpty(timeUnit))
                {
                    char lastSymbol = timeUnit[timeUnit.Length - 1];

                    if (Char.ToUpper(lastSymbol) == 'S')//plural word
                    {
                        timeUnit = timeUnit.Substring(0, timeUnit.Length - 1);

                        return ParseUnit(timeUnit);
                    }
                }

                throw new ArgumentException("Invalid time unit value");
            }

            return timeUnitEnum.ToString();
        }
    }
}
