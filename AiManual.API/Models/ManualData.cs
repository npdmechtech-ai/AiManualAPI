namespace AiManual.API.Models
{
    public class ManualData
    {
        public string? Id { get; set; }
        public string? Component { get; set; }
        public string? Type { get; set; }
        public string? Heading { get; set; }
        public List<Step>? Steps { get; set; }
    }
}