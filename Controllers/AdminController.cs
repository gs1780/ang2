using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        [HttpGet("data")]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult GetData()
        {
            return Ok(new { Message = "Secret admin data" });
        }
    }
}
