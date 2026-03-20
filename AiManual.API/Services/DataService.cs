using System.Text.Json;
using AiManual.API.Models;

namespace AiManual.API.Services
{
    public class DataService
    {
        public ManualData LoadProcedure()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "front_axle_procedure.json");

            var json = File.ReadAllText(path);

            var data = JsonSerializer.Deserialize<ManualData>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            return data ?? new ManualData();
        }
    }
}