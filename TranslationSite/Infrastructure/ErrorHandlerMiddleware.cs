using System.Net;

namespace TranslationSite.Infrastructure
{
    public class ErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlerMiddleware> _logger;

        public ErrorHandlerMiddleware(
            RequestDelegate next,
            ILogger<ErrorHandlerMiddleware> logger)
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
            catch (Exception error)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (error)
                {
                    case InvalidOperationException:
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await response.WriteAsync(error.Message);
                        break;

                    default:
                        // unhandled error
                        response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        await response.WriteAsync("Unexpected error occured");
                        break;
                }

                _logger.LogError(error.ToString());
            }
        }
    }
}
