using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
//using Twilio.Rest.Api.V2010.Account;

namespace BloodBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WhatsAppController : BaseApiController
    {
        //[HttpPost]
        //[Route("messages")]
        //public void Post([FromBody] WhatsAppMessage message)
        //{
        //    var fromNumber = message.From;
        //    var messageText = message.Text;

        //    // Use the Twilio API to send a response message to the user
        //    var responseMessage = MessageResource.Create(
        //        to: fromNumber,
        //        from: new PhoneNumber("your-whatsapp-number"),
        //        body: $"Hello, you sent: {messageText}");

        //    // Log the message for debugging purposes
        //    Debug.WriteLine($"Received message from {fromNumber}: {messageText}");
        //}
    }

    public class WhatsAppMessage
    {
        public string From { get; set; }
        public string Text { get; set; }
    }
}

