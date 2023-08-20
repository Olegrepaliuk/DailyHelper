using HelperTelegramBot.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using static System.Net.WebRequestMethods;

namespace HelperTelegramBot.BotCommands
{
    public class ExRatesCommand : ICommand
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public string CommandName => "/exrates";

        public ExRatesCommand(IConfiguration configuration, ILogger<ExRatesCommand> logger)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {           
            string answerText = string.Empty;

            try
            {
                string userInput = update.Message.Text.Substring(CommandName.Length).Trim();

                answerText = await GetCurrencyInfo(userInput);
            }
            catch (ArgumentException ex)
            {
                answerText = "Unable to show exchange rate. Please, check input data";
                _logger.LogError(ex.Message);
            }
            catch (Exception ex)
            {
                answerText = "An error occured while completing your request";
                _logger.LogError(ex.Message);
            }

            await botClient.SendTextMessageAsync(update.Message.Chat, answerText);
        }

        private async Task<string> GetCurrencyInfo (string inputMessage)
        {
            string[] currApiEndpoints = _configuration
                .GetSection("ExchangeRates:Urls")
                .GetChildren()
                .Select(child => child.Value)
                .ToArray();

            string[] commandWords = inputMessage.Split(' ');

            const int valueIndex = 0;
            const int currFromIndex = 1;
            const int currToToIndex = 3;

            string currFrom;
            string currTo;
            double currFromValue;

            try
            {
                currFromValue = Convert.ToDouble(commandWords[valueIndex]);

                currFrom = commandWords[currFromIndex];

                currTo = commandWords[currToToIndex];
            }
            catch (IndexOutOfRangeException ex)
            {
                string error = $"Unable to parse input currency exchange data. {ex?.Message}";
                throw new ArgumentException(error);
            }

            var client = new HttpClient();

            string result = string.Empty;

            foreach (string apiEndPoint in currApiEndpoints)
            {
                var response = await client.GetAsync($"{apiEndPoint}/{currFrom}/{currTo}.min.json");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsStringAsync();
                    break;
                }
            }

            if (string.IsNullOrEmpty(result)) 
            {
                throw new HttpRequestException();
            }          

            var resultJsonObject = JObject.Parse(result);

            double currToValueRate = (double)resultJsonObject[currTo];

            double currToValue = Math.Round(currToValueRate * currFromValue, 2, MidpointRounding.AwayFromZero);

            string currStrResult = $"{currFromValue} {currFrom.ToUpper()} = {currToValue} {currTo.ToUpper()}";

            return currStrResult;
        }
    }
}
