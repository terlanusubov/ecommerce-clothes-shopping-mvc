using System.Text;

namespace Comercio.Notifications.Slack
{
    public class SlackService
    {
        private readonly HttpClient _httpClient;
       
        public SlackService()
        {
            _httpClient = new HttpClient();
        }

        public async Task SendRegisterUserInfo(string message)
        {
            var uri = $"https://hooks.slack.com/services/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";

            var payload = new
            {
                text = message
            };

            var jsonPayload = Newtonsoft.Json.JsonConvert.SerializeObject(payload);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(uri, httpContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send message to Slack. Status code: {response.StatusCode}");
            }
        }

    }
}
