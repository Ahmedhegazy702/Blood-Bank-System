using System;
using BloodBank.Core;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Facebook;

namespace BloodBankSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessagesController : BaseApiController
    {
        private readonly FacebookClient _client;

        //private readonly FacebookClient _Client;
        //public MessagesController( FacebookClient client)
        //{
        //    //_Client = new FacebookClient("38afe6ae0578bc2ae488edb5e4462884", "7611171378914010");

        //}
        public MessagesController()
        {
            _client = new FacebookClient("7611171378914010");
        }



        [HttpPost("webhook")]//api/Messags/webhook
        public IActionResult Post([FromBody] JObject message)
        {
        

            try
            {
                if( message == null|| message["message"] == null || string.IsNullOrEmpty(message["message"].ToString() )) 
                {
                    return BadRequest("The message field is required.");
                }
                //parse the incoming message
                string senderId = message["from"].ToString();
                string messageText = message["text"].ToString();
                
                JObject reply = new JObject();
               
                reply["to"] = senderId;
                reply["body"] = "Thank you for your message!";
                //send the reply message
                _client.Post("me/messages", reply);
                return Ok();
            }
            catch (Exception ex)
            {
                //log the error
               Console.WriteLine("Error Processing message: "+ex.Message);

                //return an error respons 
                return BadRequest("Error processing message. ");
            }
        }
        //[HttpOptions]
        //public string GetBaseUrl()
        //{
        //    var request = HttpContext.Request;
        //    var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
        //    return baseUrl ;
        //}
    }
}
