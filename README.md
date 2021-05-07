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

