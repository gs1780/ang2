using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FlowerController : ControllerBase
    {
        private static readonly List<string> Flowers = new() { "Rose", "Tulip" };

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(Flowers);
        }

        [HttpPut]
        [Authorize(Policy = "RequireAdminRole")]
        public IActionResult Add([FromBody] string flower)
        {
            if (!string.IsNullOrWhiteSpace(flower))
            {
                Flowers.Add(flower);
            }
            return Ok(Flowers);
        }
    }
}
