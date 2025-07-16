using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthApi.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Middleware
{
    public class RoleClaimsMiddleware
    {
        private readonly RequestDelegate _next;

        public RoleClaimsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, AuthDbContext db)
        {
            if (context.User.Identity?.IsAuthenticated == true)
            {
                var email = context.User.FindFirst("preferred_username")?.Value
                            ?? context.User.FindFirst("upn")?.Value
                            ?? context.User.FindFirst(ClaimTypes.Name)?.Value;

                if (!string.IsNullOrEmpty(email))
                {
                    var roles = await db.UserRoles
                        .Where(ur => ur.User.Email == email && ur.User.IsActive)
                        .Select(ur => ur.Role.Name)
                        .ToListAsync();

                    var identity = (ClaimsIdentity)context.User.Identity;
                    foreach (var role in roles)
                    {
                        if (!identity.HasClaim(ClaimTypes.Role, role))
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, role));
                        }
                    }
                }
            }

            await _next(context);
        }
    }

    public static class RoleClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseRoleClaims(this IApplicationBuilder app)
        {
            return app.UseMiddleware<RoleClaimsMiddleware>();
        }
    }
}
