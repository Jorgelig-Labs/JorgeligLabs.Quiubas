using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using JorgeligLabs.Quiubas.Utils;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;


namespace JorgeligLabs.Quiubas.Clients
{
    public partial class QuiubasClient : IQuiubasClient
    {
        private static HttpClient _client;
        private static QuiubasOptions _options;
        protected ILogger _log;


        public QuiubasClient(IOptions<QuiubasOptions> options, HttpClient client)
        {
            _client = client;
            _options = options.Value;
            _log = Log.Logger.ForContext(this.GetType());
        }


        private async Task<TResult?> ExecuteApi<TResult>(HttpMethod method, string resourcePath,
            AuthenticationHeaderValue? authenticationHeader = null,
            object? data = null) where TResult : class
        {
            var url = GetUrl(resourcePath);
            var httpRequestMessage = new HttpRequestMessage(method, url);

            try
            {
                httpRequestMessage = AddDefaultHeaders(httpRequestMessage);
                httpRequestMessage = AddStringContent(httpRequestMessage, data);
                httpRequestMessage = AddAuthenticationHeader(httpRequestMessage, authenticationHeader);


                var httpResponse = await _client.SendAsync(httpRequestMessage);
                var resultContent = await httpResponse.Content.ReadAsStringAsync();

                if (httpResponse.IsSuccessStatusCode)
                {
                    if (string.IsNullOrWhiteSpace(resultContent)) return new ErrorResult { IsSuccessStatusCode = false } as TResult;

                    var result =
                        JsonConvert.DeserializeObject<TResult?>(resultContent, JsonUtils.StringEnumJsonSerializerSettings);

                    return result;
                }

                var error = new ErrorResult
                {
                    IsSuccessStatusCode = false,
                    Content = resultContent
                };

                return error as TResult;
            }
            catch (Exception e)
            {
                _log.Error(exception: e, $"[{method}] {resourcePath}");

                return new ErrorResult
                {
                    IsSuccessStatusCode = false,
                    Content = e.InnerException?.Message ?? e.Message
                } as TResult;
            }


            return default;
        }

        private string? GetUrl(string? resourcePath)
        {
            if (string.IsNullOrWhiteSpace(_options?.ApiBaseUrl))
                throw new ArgumentNullException(nameof(_options.ApiBaseUrl));

            if (string.IsNullOrWhiteSpace(resourcePath))
                throw new ArgumentNullException(nameof(resourcePath));

            var baseUrl = _options.ApiBaseUrl.EndsWith("/") ? _options.ApiBaseUrl : $"{_options.ApiBaseUrl}";
            var path = resourcePath.StartsWith("/") ? resourcePath : $"/{resourcePath}";
            var url = $"{baseUrl}{path}";

            return url;
        }

        private HttpRequestMessage? AddStringContent(HttpRequestMessage? requestMessage, object? data)
        {
            if (requestMessage != null && data != null)
            {
                var json = JsonConvert.SerializeObject(data, JsonUtils.StringEnumJsonSerializerSettings);
                requestMessage.Content = new StringContent(json, Encoding.UTF8, "application/json");
            }

            return requestMessage;
        }

        private HttpRequestMessage? AddAuthenticationHeader(HttpRequestMessage? requestMessage,
            AuthenticationHeaderValue? authenticationHeader)
        {
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue(
                "Basic",
                Convert.ToBase64String(Encoding.ASCII.GetBytes(_options.ApiKey + ":" + _options.ApiSecret))
            );

            return requestMessage;
        }

        private HttpRequestMessage? AddDefaultHeaders(HttpRequestMessage? requestMessage)
        {
            return requestMessage;
        }


    }

    public class ErrorResult
    {
        public bool? IsSuccessStatusCode { get; set; }
        public int? StatusCode { get; set; }
        public string? Error { get; set; }
        public string? Content { get; set; }
    }
}
