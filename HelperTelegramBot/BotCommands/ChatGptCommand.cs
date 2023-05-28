using HelperTelegramBot.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.Extensions.Configuration;
using OpenAI_API;
using Microsoft.Extensions.Logging;

namespace HelperTelegramBot.BotCommands
{
    public class ChatGptCommand : ICommand   
    {
        public string CommandName => "/chatgpt";

        private readonly IConfiguration _configuration;
        private readonly ILogger _logger;

        public ChatGptCommand(IConfiguration configuration, ILogger<ChatGptCommand> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Execute(ITelegramBotClient botClient, Update update)
        {
            
            string answerText = "";

            try
            {
                string userInput = update.Message.Text.Substring(CommandName.Length).Trim();

                string openAiApiKey = _configuration["openAi:apiKey"];
                string orgId = _configuration["openAi:orgId"];

                OpenAIAPI api = new OpenAIAPI(new APIAuthentication(openAiApiKey, orgId));

                var chat = api.Chat.CreateConversation();

                chat.AppendUserInput(userInput);

                string response = await chat.GetResponseFromChatbot();

                answerText = response;

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
