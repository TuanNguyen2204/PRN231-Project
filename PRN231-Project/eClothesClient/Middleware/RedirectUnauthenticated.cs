using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using eClothesAPI.Config;
using Microsoft.OData.ModelBuilder;

namespace eClothesClient.Middleware
{
    public class RedirectUnauthenticated : ActionFilterAttribute
    {

        private readonly IConfiguration _configuration;

        public RedirectUnauthenticated()
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

            _configuration = configBuilder.Build();
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            string routePath = context.HttpContext.Request.Path.Value.ToLower();
            if (GetRole(context).Length == 0)
            {
                // Redirect to the login page if not authenticated
                if (routePath.Contains("admin"))
                {
					context.Result = new RedirectResult("~/UnAuthorize");

					//context.Result = new RedirectResult("~/Login");
                }
                else
                {
                    context.Result = new RedirectResult("~/Login");
                }
                return;
            }
            if(GetRole(context) == "User")
            {
                if (routePath.Contains("admin"))
                {
                    context.Result = new RedirectResult("~/UnAuthorize");

                    //context.Result = new RedirectResult("~/Login");
                }
            }
            base.OnActionExecuting(context);
        }

        private string GetRole(ActionExecutingContext context)
        {
            string role = "";
            // Lấy access token từ cookie
            string accessToken = context.HttpContext.Request.Cookies["access_token"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                // Kiểm tra access token, ví dụ: kiểm tra hợp lệ và có quyền truy cập vào tài nguyên hay không
                ClaimsPrincipal claims = JWTConfig.ValidateToken(accessToken, _configuration);
                if (claims != null)
                {
                    Claim roleClaim = claims.FindFirst(c => c.Type == ClaimTypes.Role);
                    if (roleClaim != null)
                    {
                        role = roleClaim.Value;
                    }
                }
            }

            return role;
        }

    }
}
