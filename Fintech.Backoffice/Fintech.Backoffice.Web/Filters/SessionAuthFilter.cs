using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Fintech.Backoffice.Web.Filters
{
    /// <summary>
    /// Redirects anonymous users to the login page.
    /// Applied globally; only Account/* and static files are exempt.
    /// </summary>
    public class SessionAuthFilter : IActionFilter
    {
        private static readonly string[] PublicControllers = { "Account" };

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var controller = context.RouteData.Values["controller"]?.ToString() ?? "";

            // Allow public controllers
            if (PublicControllers.Contains(controller, StringComparer.OrdinalIgnoreCase))
                return;

            // Check session
            var email = context.HttpContext.Session.GetString("UserEmail");
            if (string.IsNullOrEmpty(email))
            {
                var returnUrl = context.HttpContext.Request.Path + context.HttpContext.Request.QueryString;
                context.Result = new RedirectToActionResult("Login", "Account", new { returnUrl });
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
