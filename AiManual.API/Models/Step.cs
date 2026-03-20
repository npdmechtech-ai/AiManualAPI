namespace AiManual.API.Models
{
    public class Step
    {
        public int StepNo { get; set; }
        public string? Description { get; set; }
        public List<SubStep>? SubSteps { get; set; }
        public List<Tool>? Tools { get; set; }
        public List<string>? Images { get; set; }
        public List<string>? Warnings { get; set; }
        public List<string>? Notes { get; set; }
    }
}