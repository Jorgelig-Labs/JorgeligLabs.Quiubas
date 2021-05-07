using System;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using JorgeligLabs.Quiubas.Extensions;
using Newtonsoft.Json;
using Serilog.Events;


namespace JorgeligLabs.Quiubas.Clients
{
    public partial class QuiubasClient : IQuiubasClient
    {
        private QuiubasSmsRequest ToQuiubasSmsRequest(string? toNumber, string? message)
            => new QuiubasSmsRequest
            {
                ToNumber = toNumber,
                Message = message
            };

        public async Task<QuiubasSmsResponse?> SendSms(string? toNumber, string? message)
        {
            if (string.IsNullOrWhiteSpace(toNumber))
                throw new ArgumentNullException($"{nameof(toNumber)} param is required");

            var requestData = ToQuiubasSmsRequest(toNumber, message);
            try
            {
                var response = await ExecuteApi<QuiubasSmsResponse>(HttpMethod.Post, "/sms", data: requestData);

                _log.Exit(
                    LogEventLevel.Debug,
                    arguments: new object?[] {toNumber, message},
                    returnValue: response
                );

                return response;
            }
            catch (Exception ex)
            {
                _log.Exception(LogEventLevel.Error,
                    arguments: new object?[] {toNumber, message},
                    exception: ex
                );
            }
            
            return default;
        }

        public async Task<QuiubasSmsResponse[]?> GetSms()
        {
            try
            {
                var response = await ExecuteApi<QuiubasSmsResponse[]?>(HttpMethod.Get, "/sms");
                _log.Exit(
                    LogEventLevel.Debug,
                    returnValue: response
                );

                return response;
            }
            catch (Exception ex)
            {
                _log.Exception(
                    LogEventLevel.Error,
                    exception: ex
                );
            }

            return default;
        }

    }

    public class QuiubasSmsRequest
    { 
        [JsonProperty(PropertyName = "to_number")]
        public string ToNumber { get; set; }
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

    }

    public class QuiubasSmsResponse
    {
        public string Id { get; set; }
        public string Number { get; set; }
        public string Message { get; set; }
        public DateTime? ScheduleOn { get; set; }
        public DateTime? AddedOn { get; set; }
        public DateTime? SentOn { get; set; }
        public decimal Charge { get; set; }
        public int Status { get; set; }
        public string Country { get; set; }
        public string Network { get; set; }
        public int Encode { get; set; }
        public int TestMode { get; set; }
        public bool? Error { get; set; }
    }
}
