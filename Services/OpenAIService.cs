using System.Text;
using System.Text.Json;

namespace AiManual.API.Services
{
    public class OpenAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "YOUR_API_KEY"; // keep for now

        public OpenAIService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAIResponse(string query)
        {
            try
            {
                var requestBody = new
                {
                    model = "gpt-4o-mini",
                    messages = new[]
                    {
                        new { role = "system", content = "Answer ONLY in the same language as user." },
                        new { role = "user", content = query }
                    }
                };

                var content = new StringContent(
                    JsonSerializer.Serialize(requestBody),
                    Encoding.UTF8,
                    "application/json"
                );

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_apiKey}");

                var response = await _httpClient.PostAsync(
                    "https://api.openai.com/v1/chat/completions",
                    content
                );

                var responseString = await response.Content.ReadAsStringAsync();

                // 🔥 SAFE PARSING
                using var doc = JsonDocument.Parse(responseString);

                if (doc.RootElement.TryGetProperty("choices", out var choices) &&
                    choices.GetArrayLength() > 0 &&
                    choices[0].TryGetProperty("message", out var message) &&
                    message.TryGetProperty("content", out var contentValue))
                {
                    return contentValue.GetString();
                }

                return "AI response format error.";
            }
            catch (Exception ex)
            {
                return $"AI Error: {ex.Message}";
            }
        }
    }
}