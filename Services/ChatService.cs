using AiManual.API.Models;

namespace AiManual.API.Services
{
    public class ChatService : IChatService
    {
        private readonly SearchService _searchService;

        public ChatService()
        {
            _searchService = new SearchService();
        }

        public string GetAnswer(string query)
        {
            var steps = _searchService.Search(query);
            var tools = _searchService.GetTools(query);

            if (!steps.Any())
            {
                return "No relevant data found.";
            }

            var response = "📄 Procedure:\n\n";

            int stepCounter = 1;

            foreach (var step in steps)
            {
                // Auto step numbering (FIXED ISSUE)
                response += $"Step {stepCounter}: {step.Description}\n";

                // SubSteps
                if (step.SubSteps != null && step.SubSteps.Any())
                {
                    foreach (var sub in step.SubSteps)
                    {
                        response += $"   - {sub.Description}\n";
                    }
                }

                // Images
                if (step.Images != null && step.Images.Any())
                {
                    response += "   📸 Images:\n";
                    foreach (var img in step.Images)
                    {
                        response += $"   {img}\n";
                    }
                }

                response += "\n";
                stepCounter++;
            }

            // Tools
            if (tools.Any())
            {
                response += "🔧 Tools Required:\n";
                foreach (var tool in tools)
                {
                    response += $"- {tool.Name} ({tool.Size})\n";
                }
            }

            return response;
        }
    }
}