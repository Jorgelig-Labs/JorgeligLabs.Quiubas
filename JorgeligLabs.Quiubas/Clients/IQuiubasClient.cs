using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JorgeligLabs.Quiubas.Clients
{
    /// <summary>
    /// Interface for Quiubas Client
    /// </summary>
    public interface IQuiubasClient
    {

        /// <summary>
        /// Enviar SMS <see href="https://www.quiubas.com/docs/sms/sms">Ver Documentación</see>
        /// </summary>
        /// <param name="toNumber">Numero de telefono en formato E.164 <see href="https://www.twilio.com/docs/glossary/what-e164">Ver doc.</see></param>
        /// <param name="message">Texto en el sms</param>
        /// <returns></returns>
        Task<QuiubasSmsResponse?> SendSms(string? toNumber, string? message);
        /// <summary>
        /// Recuperar los ultimos sms enviados <see href="https://www.quiubas.com/docs/sms/sms">Ver Documentación</see>
        /// </summary>
        /// <returns></returns>
        Task<QuiubasSmsResponse[]?> GetSms();

        /// <summary>
        /// Recuperar el saldo de la cuenta <see href="https://www.quiubas.com/docs/sms/balance">Ver Documentación</see>
        /// </summary>
        /// <returns></returns>
        Task<QuiubasBalanceResponse?> GetBalance();
    }
}
