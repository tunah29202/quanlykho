using FluentValidation;
using Helpers;
using Services.Common.Enums;
using Services.Core.Interfaces;
using System.Net;
using System.Text.Json;

namespace Common
{
    public class ValidationExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, ILogServices logServices, ILocalizeServices ls)
        {
            try
            {
                await _next(httpContext);
            }
            catch (ValidationException ex)
            {
                var response = new
                {
                    Title = "Validation Error",
                    Status = 400,
                    Errors = ex.Errors.Select(x => new { Field = x.PropertyName, Message = x.ErrorMessage })
                };
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsJsonAsync(response);
            }
            catch (Exception error)
            {
                var response = httpContext.Response;
                response.ContentType = "application/json";
                
                switch (error)
                {
                    case KeyNotFoundException e:
                        response.StatusCode = (int)HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        break;
                }

                if(error != null)
                {
                    string path = httpContext.Request.Path;
                    string action = httpContext.Request.Method;
                    await logServices.WriteLogException($"{action} - {path}", error.Message, error.StackTrace);
                }
                var result = JsonSerializer.Serialize(new BaseResponseError(ResponseCode.SystemError, ls.Get(Modules.Core, "Message", MessageKey.BE_003)));
                await response.WriteAsync(result);
            }
        }
    }
}
