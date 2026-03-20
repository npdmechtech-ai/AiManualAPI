using AiManual.API.Models;
using AiManual.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace AiManual.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController()
        {
            _chatService = new ChatService();
        }

        [HttpPost("ask")]
        public IActionResult Ask([FromBody] ChatRequest request)
        {
            var result = _chatService.GetAnswer(request.Query); // ✅ NO await

            return Ok(new { answer = result });
        }
    }
}