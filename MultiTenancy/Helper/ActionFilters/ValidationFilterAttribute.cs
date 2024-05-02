using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MultiTenancy.Helper.ActionFilters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

            var tenant = context.HttpContext.User.GetTenant();
            context.HttpContext.Request.Headers.TryGetValue("tenant", out var tenantId);
            if (tenant != tenantId)
            {
                context.Result = new UnauthorizedObjectResult("Login User does not belong to this tenant!");
                return;
            }

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var tenant = context.HttpContext.User.GetTenant();
            context.HttpContext.Request.Headers.TryGetValue("tenant", out var tenantId);
            if (tenant != tenantId)
            {
                context.Result = new UnauthorizedObjectResult("Login User does not belong to this tenant!");
                return;
            }
        }
    }
}
