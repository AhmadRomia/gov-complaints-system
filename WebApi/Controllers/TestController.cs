using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("throw")]
        public IActionResult Throw()
        {
            throw new Exception("🚀 Test Telegram Exception - System verification in progress.");
        }
    }
}
