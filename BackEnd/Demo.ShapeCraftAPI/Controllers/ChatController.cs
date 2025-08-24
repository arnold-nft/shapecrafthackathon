using Microsoft.AspNetCore.Mvc;
using ShapeCraft.AzureAISearch.Services.Contracts; 

namespace Demo.ShapeCraftAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IRagChatService _ragChatService;

        public ChatController(IRagChatService ragChatService)
        {
            _ragChatService = ragChatService;
        }

        [HttpGet("generate")]
        public async Task<IActionResult> AskQuestion(string question)
        {
            try
            {
                string answer = await _ragChatService.AskAsync(question);

                return Ok(new { answer });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }
    }
}
