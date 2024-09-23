using BloodBank.Core.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BloodBank.Service
{
    public class MailboxEmailVerification : IEmailVerificationService
    {
        private readonly HttpClient _httpClient;

        public MailboxEmailVerification(HttpClient httpClient)
        {
           _httpClient = httpClient;
        }
        public async Task<bool> IsValidEmailAsync(string email)
        {
            var response = await _httpClient.GetAsync($"http://apilayer.net/api/check?access_key=YOUR_ACCESS_KEY&email={email}$smtp=1&format=1");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<MailboxLayerResponse>(content);
            return result.IsValidEmail;
        }
        private class MailboxLayerResponse
        {
            [JsonProperty("valid")]
            public bool IsValidEmail { get; set; }

        }
    }
}
