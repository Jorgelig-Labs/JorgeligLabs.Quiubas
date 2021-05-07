using System;
using System.Net.Http;
using JorgeligLabs.Quiubas.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;
using Microsoft.Extensions.Configuration.UserSecrets ;

namespace JorgeligLabs.Quiubas.Test
{
    public class QuiubasClientTest
    {
        private readonly IQuiubasClient _client;
        public static IConfigurationRoot Configuration { get; set; }
        /// <summary>
        /// https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows
        /// </summary>
        private string? ApiBaseUrl => Configuration["AppSettings:QuiubasOptions:ApiBaseUrl"] ?? String.Empty;
        private string? ApiKey => Configuration["AppSettings:QuiubasOptions:ApiKey"] ?? String.Empty;
        private string? ApiSecret => Configuration["AppSettings:QuiubasOptions:ApiSecret"] ?? String.Empty;

        public QuiubasClientTest()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<QuiubasClientTest>();
            Configuration = builder.Build();

            var options = Options.Create(new QuiubasOptions()
            {
                ApiBaseUrl = ApiBaseUrl,
                ApiKey = ApiKey,
                ApiSecret = ApiSecret
            });

            _client = new QuiubasClient(options, new HttpClient());
        }

        [Fact]
        public void Sms_SendSms_MessageSent()
        {
            string? toNumber = "+528114117118";
            string? message = "Hello, World!";

            var response =_client.SendSms(toNumber, message);
            
            Assert.NotNull(response?.Result);
            Assert.False(response?.Result?.Error);
            Assert.NotEmpty(response?.Result?.Id ?? "");

        }

        [Fact]
        public void Sms_GetSms_ReturnsCollection()
        {
            var response = _client.GetSms();

            Assert.NotNull(response?.Result);
            Assert.True((response?.Result?.Length ?? 0) > 0);
        }

        [Fact]
        public void Sms_GetBalance_RetrieveAccountBalance()
        {
            var response = _client.GetBalance();

            Assert.NotNull(response?.Result);
            Assert.NotEmpty(response?.Result.Currency);
            Assert.True((response?.Result.Balance ?? 0) > 0);
        }
    }
}
