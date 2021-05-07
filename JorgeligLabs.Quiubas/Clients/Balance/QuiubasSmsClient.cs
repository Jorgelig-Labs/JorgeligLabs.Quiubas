using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JorgeligLabs.Quiubas.Extensions;
using Serilog.Events;

namespace JorgeligLabs.Quiubas.Clients
{
    public partial class QuiubasClient : IQuiubasClient
    {
        public async Task<QuiubasBalanceResponse?> GetBalance()
        {
            try
            {
                var response = await ExecuteApi<QuiubasBalanceResponse?>(HttpMethod.Get, "/balance ");
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



    public class QuiubasBalanceResponse
    {
        public bool? Error { get;set; }
        /// <summary>
        /// Valor decimal del saldo de la cuenta
        /// </summary>
        public decimal? Balance { get; set; }
        /// <summary>
        /// Nomenclatura de la moneda del saldo, por ejemplo MXN
        /// </summary>
        public string? Currency { get; set; }
    }

}
