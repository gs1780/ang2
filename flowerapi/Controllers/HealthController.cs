using System;
using System.Data.Common;
using System.Threading.Tasks;
using AuthApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HealthController : ControllerBase
    {
        private readonly ILogger<HealthController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AuthDbContext _dbContext;

        public HealthController(ILogger<HealthController> logger, IConfiguration configuration, AuthDbContext dbContext)
        {
            _logger = logger;
            _configuration = configuration;
            _dbContext = dbContext;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("Health endpoint called");

            var connStr = _configuration.GetConnectionString("DefaultConnection") ?? string.Empty;
            var sanitized = SanitizeConnectionString(connStr);
            _logger.LogInformation("DB Connection String: {ConnectionString}", sanitized);

            bool dbConnectionSuccess;
            try
            {
                dbConnectionSuccess = await _dbContext.Database.CanConnectAsync();
            }
            catch (Exception ex)
            {
                dbConnectionSuccess = false;
                _logger.LogError(ex, "Database connection failed");
            }

            _logger.LogInformation("DB connection success: {Success}", dbConnectionSuccess);

            return Ok(new
            {
                health = "ok",
                dbConnectionString = sanitized,
                dbConnectionSuccess
            });
        }

        private static string SanitizeConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                return connectionString;
            }

            var builder = new DbConnectionStringBuilder { ConnectionString = connectionString };
            if (builder.ContainsKey("Password"))
            {
                builder["Password"] = "******";
            }
            if (builder.ContainsKey("Pwd"))
            {
                builder["Pwd"] = "******";
            }
            return builder.ConnectionString;
        }
    }
}

