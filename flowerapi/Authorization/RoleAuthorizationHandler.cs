using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthApi.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace AuthApi.Authorization
{
    public class RoleAuthorizationHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly AuthDbContext _dbContext;

        public RoleAuthorizationHandler(AuthDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            var email = context.User.FindFirst("preferred_username")?.Value
                         ?? context.User.FindFirst("upn")?.Value
                         ?? context.User.FindFirst(ClaimTypes.Name)?.Value;

            if (string.IsNullOrEmpty(email))
            {
                return;
            }

            var user = await _dbContext.Users
                .Include(u => u.UserRoles)
                    .ThenInclude(ur => ur.Role)
                .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);

            if (user == null)
            {
                return;
            }

            if (user.UserRoles.Any(ur => ur.Role.Name == requirement.RoleName))
            {
                context.Succeed(requirement);
            }
        }
    }
}
