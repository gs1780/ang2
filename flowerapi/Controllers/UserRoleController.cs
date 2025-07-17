using System.Linq;
using AuthApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly AuthDbContext _dbContext;

        public UserRoleController(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Get()
        {
            var users = _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                })
                .ToList();

            return Ok(users);
        }
    }
}
