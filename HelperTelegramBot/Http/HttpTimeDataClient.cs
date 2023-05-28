using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using TimeService.Models;

namespace HelperTelegramBot.Http
{
    public class HttpTimeDataClient: ITimeDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _timeServiceUri;
        private readonly ILogger _logger;

        public HttpTimeDataClient(HttpClient httpClient, IConfiguration configuration, ILogger<HttpTimeDataClient> logger) 
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _timeServiceUri = _configuration["TimeServiceURI"];
            _logger = logger;
        }

        public async Task<DayInfoResponse> GetDayInfo(DateTime day)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(new DayInfoRequest { Day = day}),
                Encoding.UTF8,
                "application/json");

            var requestUri = $"{_timeServiceUri}/info";

            try
            {
                var token = await AcquireTokenWithSecret();

                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token.CreateAuthorizationHeader());

                var response = await _httpClient.PostAsync(requestUri, httpContent);

                var responseContent = await response.Content.ReadAsStringAsync();

                var dayInfo = JsonSerializer.Deserialize<DayInfoResponse>(responseContent);

                return dayInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }                      
        }

        public async Task<ConvertTimeResponse> ConvertTime(string unitFrom, string unitTo, double value)
        {
            var convertTimeRequest = new ConvertTimeRequest
            {
                UnitFrom = unitFrom,
                UnitTo = unitTo,
                Value = value
            };

            var httpContent = new StringContent(
                JsonSerializer.Serialize(convertTimeRequest),
                Encoding.UTF8,
                "application/json");

            var requestUri = $"{_timeServiceUri}/convert";

            try
            {
                var token = await AcquireTokenWithSecret();

                _httpClient.DefaultRequestHeaders.Authorization = AuthenticationHeaderValue.Parse(token.CreateAuthorizationHeader());

                var response = await _httpClient.PostAsync(requestUri, httpContent);

                var responseContent = await response.Content.ReadAsStringAsync();

                var convertResult = JsonSerializer.Deserialize<ConvertTimeResponse>(responseContent);

                return convertResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
        }

        private Task<AuthenticationResult> AcquireTokenWithSecret()
        {
            string secret = _configuration["appReg:clientSecret"];
            string clientId = _configuration["appReg:clientId"];
            string scope = _configuration["appReg:scope"];
            string authorityURI = $"{_configuration["appReg:authorityURI"]}/{_configuration["appReg:tenant"]}";

            var app = ConfidentialClientApplicationBuilder.Create(clientId).WithAuthority(authorityURI).WithClientSecret(secret).Build();
            var scopes = new[] { scope };
            return app.AcquireTokenForClient(scopes).ExecuteAsync(CancellationToken.None);
        }
    }
}
