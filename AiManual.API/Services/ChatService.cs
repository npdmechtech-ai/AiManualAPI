using AiManual.API.Models;
using System.Text;

namespace AiManual.API.Services
{
    public class ChatService
    {
        private readonly SearchService _searchService;
        private readonly OpenAIService _openAIService;

        public ChatService(SearchService searchService, OpenAIService openAIService)
        {
            _searchService = searchService;
            _openAIService = openAIService;
        }

        public async Task<string> GetAnswer(string query)
        {
            if (string.IsNullOrEmpty(query))
                return "Invalid query.";

            // 🔥 Detect language
            var language = DetectLanguage(query);

            // 🔥 Convert to English for search
            var searchQuery = await ConvertToEnglish(query);

            // 🔥 Search steps
            var steps = _searchService.Search(searchQuery);

            if (steps == null || steps.Count == 0)
                return "No relevant data found.";

            // 🔥 Build context with IMAGES
            var contextBuilder = new StringBuilder();

            foreach (var step in steps.Take(8))
            {
                if (!string.IsNullOrEmpty(step.Description))
                    contextBuilder.AppendLine($"Step {step.StepNo}: {step.Description}");

                // SubSteps
                if (step.SubSteps != null)
                {
                    foreach (var sub in step.SubSteps)
                    {
                        if (!string.IsNullOrEmpty(sub.Description))
                            contextBuilder.AppendLine($"   - {sub.Description}");

                        // 🔥 Substep Images
                        if (sub.Images != null)
                        {
                            foreach (var img in sub.Images)
                            {
                                var url = $"https://aimanual-images.s3.amazonaws.com/Procedures/{img}";
                                contextBuilder.AppendLine($"Image: {url}");
                            }
                        }
                    }
                }

                // 🔥 Step Images
                if (step.Images != null)
                {
                    foreach (var img in step.Images)
                    {
                        var url = $"https://aimanual-images.s3.amazonaws.com/Procedures/{img}";
                        contextBuilder.AppendLine($"Image: {url}");
                    }
                }
            }

            // 🔥 AI Response
            var aiResponse = await _openAIService.GetAIResponse(
                query,
                contextBuilder.ToString(),
                language
            );

            return aiResponse;
        }

        // 🔥 Language detection
        private string DetectLanguage(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "English";

            if (text.Any(c => c >= 0x0B80 && c <= 0x0BFF))
                return "Tamil";

            if (text.Any(c => c >= 0x0900 && c <= 0x097F))
                return "Hindi";

            return "English";
        }

        // 🔥 Translate to English
        private async Task<string> ConvertToEnglish(string text)
        {
            var language = DetectLanguage(text);

            if (language == "English")
                return text;

            var translated = await _openAIService.GetAIResponse(
                text,
                "Translate this to English. Only give translated sentence.",
                "English"
            );

            return translated;
        }
    }
}