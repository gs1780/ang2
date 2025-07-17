using System.Linq;
using System;
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
            string sqlQuery = string.Empty;
            try
            {
                var query = _dbContext.Users
                    .Include(u => u.UserRoles)
                        .ThenInclude(ur => ur.Role)
                    .Select(u => new
                    {
                        u.Id,
                        u.Email,
                        Roles = u.UserRoles.Select(ur => ur.Role.Name).ToList()
                    });

                sqlQuery = query.ToQueryString();
                var users = query.ToList();

                return Ok(new { Data = users, SqlQuery = sqlQuery });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { SqlQuery = sqlQuery, Error = ex.ToString() });
            }
        }
    }
}
