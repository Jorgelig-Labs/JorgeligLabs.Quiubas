# Quiubas SDK
Cliente .NET para implementar api de [quiubas](https://www.quiubas.com/) 

## Lista de métodos disponibles  

### Enviar sms  

**URL del Recurso**  
POST /sms  
  
  
**Parámetros para la consulta**
| Parámetro      | Descripción |
| ----------- | ----------- |
| toNumber      | Numero de telefono en formato [E.164](https://www.twilio.com/docs/glossary/what-e164)       |
| message   | Texto en el sms        |

### Recuperar los SMS enviados mas recientemente

**URL del Recurso**  
GET /sms  

### Obtener el saldo de la cuenta

**URL del Recurso**  
/balance

| Parámetro      | Descripción |
| ----------- | ----------- |
| toNumber      | Numero de telefono en formato [E.164](https://www.twilio.com/docs/glossary/what-e164)       |
| message   | Texto en el sms        |



### Ejemplos de uso
```csharp

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

```
