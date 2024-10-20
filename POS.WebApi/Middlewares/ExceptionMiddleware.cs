using POS.Application.Exceptions;
using POS.WebApi.Errors;
using System.Net;
using System.Text.Json;

namespace POS.WebApi.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                var statusCode = ex is NotFoundException ? HttpStatusCode.NotFound :
                                 ex is ValidationException || ex is BadRequestException ? HttpStatusCode.BadRequest :
                                 HttpStatusCode.InternalServerError;
                IEnumerable<KeyValuePair<string, string>>? messageList = null;

                var message = ex.Message;
                if (ex is ValidationException ve)
                {
                    if (ve.Errors.Count() == 1)
                    {
                        message = ve.Errors.First().Value.FirstOrDefault();
                    }
                    else
                    {
                        message = string.Empty;
                        messageList = ve.Errors.Select(x => new KeyValuePair<string, string>(x.Key, string.Join(". ", x.Value)));
                    }
                }

                var result = JsonSerializer.Serialize(new CodeErrorResponse((int)statusCode, message, ex.StackTrace, messageList));

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                await context.Response.WriteAsync(result);
            }
        }
    }
}
