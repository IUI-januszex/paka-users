using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace PakaUsers
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(RequestDelegate next, ILogger<ErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
           
            catch (Exception ex)
            {
                var errorResponse = new {Message = "Unhandled server error, contact system admin if error persists"};
                _logger.LogError(ex.Message);
                _logger.LogError(ex.StackTrace);
                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
            }
        }

    }
}