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
            if (string.IsNullOrEmpty(query))
                return new List<Step>();

            var keywords = query.ToLower().Split(' ');

            if (_data?.Steps == null)
                return new List<Step>();

            return _data.Steps
                .Where(step =>
                    keywords.Any(k =>
                        (!string.IsNullOrEmpty(step.Description) &&
                         step.Description.ToLower().Contains(k)) ||

                        (step.SubSteps != null &&
                         step.SubSteps.Any(sub =>
                             !string.IsNullOrEmpty(sub.Description) &&
                             sub.Description.ToLower().Contains(k)))
                    )
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
                .GroupBy(t => t.Name + t.Size)
                .Select(g => g.First())
                .ToList();
        }
    }
}