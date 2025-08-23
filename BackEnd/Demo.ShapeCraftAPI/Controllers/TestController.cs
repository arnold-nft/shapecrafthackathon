using Microsoft.AspNetCore.Mvc;

namespace Demo.ShapeCraftAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("GetData")]
        public async Task<IActionResult> GetData()
        {
            try
            {
                var result = "TEST";
                return Ok(new { Data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }


    }
}
