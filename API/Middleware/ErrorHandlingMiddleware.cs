using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Errors;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private const string ResponseContentType = "application/json";
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware([NotNull] RequestDelegate next, [NotNull] ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                await HandleException(e, context);
            }
        }

        private async Task HandleException(Exception exception, HttpContext context)
        {
            object errors;
            switch (exception)
            {
                case RestException re:
                    _logger.LogError(exception, "REST ERROR");
                    errors = re.Errors;
                    context.Response.StatusCode = (int) re.StatusCode;
                    break;
                default:
                    _logger.LogError(exception, "SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
                    context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;
                    break;
            }

            context.Response.ContentType = ResponseContentType;
            if (errors != null)
            {
                var json = JsonSerializer.Serialize(new {errors});
                await context.Response.WriteAsync(json);
            }
        }
    }
}