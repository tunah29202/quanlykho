using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Helpers.Auth;
using Microsoft.Extensions.Options;
using Services.Core.Contracts;
using Services.Core.Interfaces;
using Newtonsoft.Json;
using Services.Common.Enums;
using Helpers;
namespace Common
{
    public class ApiAuthorize : TypeFilterAttribute
    {
        public ApiAuthorize() : base(typeof(AuthorizeAttribute))
        {
        }
        private class AuthorizeAttribute : Attribute, IActionFilter, IAsyncActionFilter
        {
            private readonly IAuthServices _authServices;
            private readonly IOptions<AuthSetting> _settings;
            private readonly ILocalizeServices _ls;
            private readonly ILogServices _logServices;
            public AuthorizeAttribute(IAuthServices authServices, IOptions<AuthSetting> settings, ILocalizeServices ls, ILogServices logServices)
            {
                _authServices = authServices;
                _settings = settings;
                _ls = ls;
                _logServices = logServices;
            }
            public void OnActionExecuting(ActionExecutingContext context)
            {
                //CheckUserPermission(context);
            }
            public void OnActionExecuted(ActionExecutedContext context)
            {
                // Do something after the action executes.
            }
            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {
                OnActionExecuting(context);
                await WriteLogAction(context);
                await next();
            }
            #region Private method
            private void CheckUserPermission(ActionExecutingContext context)
            {
                HttpRequest httpRequest = context.HttpContext.Request;
                string path = context.ActionDescriptor.AttributeRouteInfo.Template;
                string action = httpRequest.Method;

                var bearerToken = httpRequest.Headers["Authorization"];
                var token = !string.IsNullOrEmpty(bearerToken) ? bearerToken.ToString().Substring("Bearer ".Length) : null;
                var user_id = JwtHelpers.ValidateToken(token, _settings.Value.JWTSecret);
                if (user_id == null)
                {
                    context.Result = new UnauthorizedObjectResult(new { ResponseCode.UnAuthorized, message = _ls.Get(Modules.Core, "Common", "NotHavePermission") });
                    return;
                }
                var check_auth = _authServices.CheckUserAuthorized((Guid)user_id, path, action);
                if(!check_auth)
                {
                    context.Result = new UnauthorizedObjectResult(new { code = ResponseCode.UnAuthorized, message = _ls.Get(Modules.Core, "Common", "NotHavePermission") });
                    return;
                }
            }
            private async Task WriteLogAction(ActionExecutingContext context)
            {
                HttpRequest httpRequest = context.HttpContext.Request;
                string path = context.ActionDescriptor.AttributeRouteInfo.Template;
                string action = httpRequest.Method;
                if (!string.IsNullOrEmpty(action) && action.ToUpper() != "Get")
                {
                    string body = string.Empty;
                    foreach (var item in context.ActionArguments)
                    {
                        body += $"{item.Key}: {JsonConvert.SerializeObject(item.Value)}\n";
                    }
                    await _logServices.WriteLogAction(path, action, body);
                }
            }
            #endregion
        }
    }
}