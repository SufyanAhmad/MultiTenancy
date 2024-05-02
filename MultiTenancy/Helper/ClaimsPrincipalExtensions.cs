using System.Security.Claims;

namespace MultiTenancy.Helper
{
    public static class ClaimsPrincipalExtensions
    {
        public static string GetUserId(this ClaimsPrincipal principal)
        {
            var user = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            return user;
        }
        public static string GetTenant(this ClaimsPrincipal principal)
        {
            var tenant = principal.FindFirstValue(ClaimTypes.System);
            return tenant;
        }
    }
}
