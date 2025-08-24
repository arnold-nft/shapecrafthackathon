
using Demo.ShapeCraftAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace Demo.ShapeCraftAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly TokenService _tokenService;

        public TokenController(TokenService tokenService)
        {
            _tokenService = tokenService;
        }

        [HttpPost("generate")]
        public IActionResult GenerateToken([FromBody] string userAddress)
        {
            if (string.IsNullOrEmpty(userAddress))
            {
                return BadRequest("User address is required.");
            }

            var token = _tokenService.GenerateToken(userAddress);
            return Ok(new { token });
        }
    }
}

