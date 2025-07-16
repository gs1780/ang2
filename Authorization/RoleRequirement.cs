using Microsoft.AspNetCore.Authorization;

namespace AuthApi.Authorization
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string RoleName { get; }

        public RoleRequirement(string roleName)
        {
            RoleName = roleName;
        }
    }
}
