using Microsoft.AspNetCore.Mvc;
using AiManual.API.Services;
using AiManual.API.Models;

namespace AiManual.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly ChatService _chatService;

        public ChatController(ChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> Ask([FromBody] ChatRequest request)
        {
            var result = await _chatService.GetAnswer(request.Query);
            return Ok(new { answer = result });
        }
    }
}