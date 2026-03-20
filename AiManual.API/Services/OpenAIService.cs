using System.Text;
using System.Text.Json;

namespace AiManual.API.Services
{
    public class OpenAIService
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public OpenAIService(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient();
        }

        public async Task<string> GetAIResponse(string query, string context, string language)
        {
            var apiKey = _config["OpenAI:ApiKey"];

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

            var prompt = $@"
User Question: {query}

Context:
{context}

STRICT RULE:
- Answer ONLY in {language}
- Do NOT change language
- Do NOT mix languages

IMPORTANT:
- Show images under each step if available
- Format:
  Step 1:
  Description...
  Images:
  <url>

Instructions:
- Step-by-step explanation
- Use only given context
";

            var requestBody = new
            {
                model = "gpt-4o-mini",
                messages = new[]
                {
                    new { role = "system", content = "You are a strict multilingual technical assistant." },
                    new { role = "user", content = prompt }
                },
                temperature = 0.2
            };

            var json = JsonSerializer.Serialize(requestBody);

            var response = await _httpClient.PostAsync(
                "https://api.openai.com/v1/chat/completions",
                new StringContent(json, Encoding.UTF8, "application/json")
            );

            var result = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(result);

            return doc.RootElement
                      .GetProperty("choices")[0]
                      .GetProperty("message")
                      .GetProperty("content")
                      .GetString();
        }
    }
}