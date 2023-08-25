# DailyHelper

Project Description: This project is designed to assist users in obtaining various types of information, including dates, unit conversions, currency rates, and interacting with artificial intelligence. It consists of several applications working together.

## Tech Stack

- .NET 7
- ASP.NET Web API
- Azure
- Azure Cosmos DB

## Applications

### HelperTelegramBot

The `HelperTelegramBot` is a C# application that serves as a bot for the Telegram messenger platform. It interacts with external APIs to retrieve information and sends messages to users. The bot provides the following commands:

- `/dayinfo`: Retrieves information about the current date using the TimeService API.
- `/exrates`: Provides exchange rates for selected currencies.
- `/chatgpt`: Allows users to query the GPT 3.5 model using the OpenAI API.
- `/convert`: Converts values to other units using the TimeService API.

### TimeService

The `TimeService` is an ASP.NET Web API application that offers various methods for retrieving time-related information. Some of the available methods include:

- `GetYearInfo`: Retrieves information about the current year.
- `SubtractDates`: Calculates the time difference between two dates.
- `GetDayInfo`: Provides information about the current date.
- `ConvertTimeUnit`: Converts a value to other units using the TimeService API.