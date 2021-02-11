using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;

namespace dotnetpostgres.Api.Middlewares
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestMiddleware> _logger;
        private readonly IConfiguration _configuration;

        // Must have constructor with this signature, otherwise exception at run time
        public RequestMiddleware(RequestDelegate next, ILogger<RequestMiddleware> logger, IConfiguration configuration)
        {
            // This is an HTTP Handler, so no need to store next
            _next = next;

            _logger = logger;
            _configuration = configuration;
        }

        public async Task Invoke(HttpContext context)
        {
            bool hasError = false;
            bool logRequestContent = false;

            string requestContent = string.Empty;
            var ip = string.Empty;

            context.Request.EnableBuffering();

            Stream originalBody = context.Response.Body;

            try
            {
                //get we really need to log request content or not from appSettings
                var setting = _configuration["LogRequestContent"];

                logRequestContent = setting != null && setting.ToLower() == "true";

                if (logRequestContent)
                {
                    var address = context.Request.HttpContext.Connection.RemoteIpAddress;

                    ip = address?.ToString();

                    using var reader = new StreamReader(
                        context.Request.Body,
                        Encoding.UTF8,
                        false,
                        leaveOpen: true);
                    requestContent = await reader.ReadToEndAsync();
                    // Do some processing with body…

                    // Reset the request body stream position so the next middleware can read it
                    context.Request.Body.Position = 0;
                }

                await _next(context);
            }
            catch (Exception exc)
            {
                hasError = true;
                context.Response.Clear();
                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json; charset=utf-8";

                //set application exception return obj
                JObject o = JObject.FromObject(new
                {
                    type_text = ResponseType.InternalError.ToString(),
                    error_code = ErrorCode.ApplicationException,
                    type = ResponseType.InternalError
                });

                await context.Response.WriteAsync(o.ToString());

                //log service exc
                _logger.LogError(exc, "Unhandled error", null);
            }
            finally
            {
                if (!hasError)
                {
                    context.Response.Body = originalBody;
                }

                if (logRequestContent)
                {
                    LogRequestAndResponse(context.Response.StatusCode, requestContent, ip);
                }
            }
        }

        /// <summary>
        /// logs req and resp
        /// </summary>
        /// <param name="responseStatusCode"></param>
        /// <param name="requestContent"></param>
        /// <param name="ip"></param>
        private void LogRequestAndResponse(int responseStatusCode, string requestContent, string ip)
        {
            //trim requestContent as max 500 char variable
            var content = string.IsNullOrEmpty(requestContent)
                ? null
                : requestContent.Length >= 500 ? requestContent.Substring(0, 500) : requestContent;

            var logContent = string.Format("ip: {0}|status: {1}|req: {2}", ip, responseStatusCode, content);

            _logger.LogInformation(logContent);
        }

    }

    public static class RequestMiddlewareExtensions
    {
        public static IApplicationBuilder UseRequestMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestMiddleware>();
        }
    }

}
