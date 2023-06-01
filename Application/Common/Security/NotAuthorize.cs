using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Application.Common.Security;

public class NotAuthorize: TypeFilterAttribute
{
    public NotAuthorize() : base(typeof(CustomAuthorizationFilter))
    {
    }

    private class CustomAuthorizationFilter : IAuthorizationFilter
    {

        public CustomAuthorizationFilter()
        {
            
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            // Perform your custom authorization logic here
            // Example: Check if the user has a specific role or meets certain requirements
            var isAuthorized = CheckCustomAuthorization(context.HttpContext.User);
    
            if (isAuthorized)
            {
                context.HttpContext.Response.Redirect(context.HttpContext.Request.PathBase + "/");
            } 
        }

        private bool CheckCustomAuthorization(ClaimsPrincipal user)
        {
            return user.Identity != null ? user.Identity.IsAuthenticated : false;
        }
    }
}