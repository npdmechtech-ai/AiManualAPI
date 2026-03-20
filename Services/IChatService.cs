using AiManual.API.Models;

namespace AiManual.API.Services
{
    public interface IChatService
    {
        string GetAnswer(string query);
    }
}