using AiManual.API.Models;

namespace AiManual.API.Services
{
    public class SearchService
    {
        private readonly ManualData _data;

        public SearchService()
        {
            var dataService = new DataService();
            _data = dataService.LoadProcedure();
        }

        public List<Step> Search(string query)
        {
            if (string.IsNullOrEmpty(query) || _data?.Steps == null)
                return new List<Step>();

            query = query.ToLower();

            // 🔥 Split query into keywords
            var keywords = query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Where(k => k.Length > 2) // ignore small words like "to", "is"
                .ToList();

            return _data.Steps
                .Where(step =>
                    // ✅ Match Step Description
                    keywords.Any(k =>
                        !string.IsNullOrEmpty(step.Description) &&
                        step.Description.ToLower().Contains(k)
                    )

                    ||

                    // ✅ Match SubSteps
                    (step.SubSteps != null &&
                     step.SubSteps.Any(sub =>
                         keywords.Any(k =>
                             !string.IsNullOrEmpty(sub.Description) &&
                             sub.Description.ToLower().Contains(k)
                         )
                     ))
                )
                .ToList();
        }

        public List<Tool> GetTools(string query)
        {
            var steps = Search(query);

            var tools = new List<Tool>();

            foreach (var step in steps)
            {
                if (step.Tools != null && step.Tools.Count > 0)
                {
                    tools.AddRange(step.Tools);
                }
            }

            return tools
                .Where(t => !string.IsNullOrEmpty(t.Name))
                .GroupBy(t => (t.Name + t.Size).ToLower())
                .Select(g => g.First())
                .ToList();
        }
    }
}