using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
public class AdminAccessAttribute : Attribute, IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var role = context.HttpContext.Session?.GetString("Role");

        if (string.IsNullOrEmpty(role) || !role.Equals("1", StringComparison.OrdinalIgnoreCase))
        {
            // Redirect or handle unauthorized access as needed
            context.Result = new RedirectToActionResult("Index", "Home", null);
        }
    }
}
